// ==========================================================
// FreeImage 3 .NET wrapper
// Original FreeImage 3 functions and .NET compatible derived functions
//
// Design and implementation by
// - Jean-Philippe Goerke (jpgoerke@users.sourceforge.net)
// - Carsten Klein (cklein05@users.sourceforge.net)
//
// Contributors:
// - David Boland (davidboland@vodafone.ie)
//
// Main reference : MSDN Knowlede Base
//
// This file is part of FreeImage 3
//
// COVERED CODE IS PROVIDED UNDER THIS LICENSE ON AN "AS IS" BASIS, WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, WITHOUT LIMITATION, WARRANTIES
// THAT THE COVERED CODE IS FREE OF DEFECTS, MERCHANTABLE, FIT FOR A PARTICULAR PURPOSE
// OR NON-INFRINGING. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE COVERED
// CODE IS WITH YOU. SHOULD ANY COVERED CODE PROVE DEFECTIVE IN ANY RESPECT, YOU (NOT
// THE INITIAL DEVELOPER OR ANY OTHER CONTRIBUTOR) ASSUME THE COST OF ANY NECESSARY
// SERVICING, REPAIR OR CORRECTION. THIS DISCLAIMER OF WARRANTY CONSTITUTES AN ESSENTIAL
// PART OF THIS LICENSE. NO USE OF ANY COVERED CODE IS AUTHORIZED HEREUNDER EXCEPT UNDER
// THIS DISCLAIMER.
//
// Use at your own risk!
// ==========================================================

// ==========================================================
// CVS
// $Revision$
// $Date$
// $Id$
// ==========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FreeImageAPI
{
	public class ImageMetadata : IEnumerable, IComparable, IComparable<ImageMetadata>
	{
		private readonly List<MetadataModel> data;
		private readonly FIBITMAP dib;
		private bool hideEmptyModels;

		/// <summary>
		/// Creates a new ImageMetadata instance, showing all known models.
		/// </summary>
		/// <param name="dib">Handle to a FreeImage bitmap.</param>
		public ImageMetadata(FIBITMAP dib) : this(dib, false) { }

		/// <summary>
		/// Creates a new ImageMetadata instance.
		/// </summary>
		/// <param name="dib">Handle to a FreeImage bitmap.</param>
		/// <param name="hideEmptyModels">When true, empty metadata models
		/// will be hidden until a tag to this model is added.</param>
		public ImageMetadata(FIBITMAP dib, bool hideEmptyModels)
		{
			if (dib.IsNull) throw new ArgumentNullException("dib");
			data = new List<MetadataModel>(FreeImage.FREE_IMAGE_MDMODELS.Length);
			this.dib = dib;
			this.hideEmptyModels = hideEmptyModels;

			foreach (Type exportedType in Assembly.GetAssembly(this.GetType()).GetExportedTypes())
			{
				if (exportedType.IsClass &&
					exportedType.IsPublic &&
					exportedType.BaseType != null &&
					exportedType.BaseType == typeof(MetadataModel))
				{
					ConstructorInfo constructorInfo = exportedType.GetConstructor(new Type[] { typeof(FIBITMAP) });
					if (constructorInfo != null)
					{
						MetadataModel model = (MetadataModel)constructorInfo.Invoke(new object[] { dib });
						if (model != null)
						{
							data.Add(model);
						}
					}
				}
			}
			data.Capacity = data.Count;
		}

		/// <summary>
		/// Gets or sets the MetadataModel of the specified type.
		/// <para>In case the getter returns null the model is not contained
		/// by the list.</para>
		/// <para>'null' can be used calling the setter to destroy the model.</para>
		/// </summary>
		/// <param name="model">Type of the model.</param>
		/// <returns>The MetadataModel object of the specified type.</returns>
		public MetadataModel this[FREE_IMAGE_MDMODEL model]
		{
			get
			{
				for (int i = 0; i < data.Count; i++)
				{
					if (data[i].Model == model)
					{
						if (!data[i].Exists && hideEmptyModels)
							return null;
						return data[i];
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets or sets the MetadataModel at the specified index.
		/// <para>In case the getter returns null the model is not contained
		/// by the list.</para>
		/// <para>'null' can be used calling the setter to destroy the model.</para>
		/// </summary>
		/// <param name="index">Index of the MetadataModel within this instance.</param>
		/// <returns>The MetadataModel object at the specified index.</returns>
		public MetadataModel this[int index]
		{
			get
			{
				if (index < 0 || index >= data.Count) throw new ArgumentOutOfRangeException("index");
				return (hideEmptyModels && !data[index].Exists) ? null : data[index];
			}
		}

		/// <summary>
		/// Returns a list of all visible metadata models.
		/// </summary>
		public List<MetadataModel> List
		{
			get
			{
				if (hideEmptyModels)
				{
					List<MetadataModel> result = new List<MetadataModel>();
					for (int i = 0; i < data.Count; i++)
						if (data[i].Exists)
							result.Add(data[i]);
					return result;
				}
				else
				{
					return data;
				}
			}
		}

		/// <summary>
		/// Adds new tag to the bitmap
		/// or updates its value in case it already exists.
		/// 'tag.Key' will be used as key.
		/// </summary>
		/// <param name="tag">The tag to add or update.</param>
		/// <returns>Returns true on success, false on failure.</returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown in case 'tag' is null.</exception>
		public bool AddTag(MetadataTag tag)
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (tag.Model == data[i].Model)
				{
					return data[i].AddTag(tag);
				}
			}
			return false;
		}

		/// <summary>
		/// Returns the number of visible metadata models.
		/// </summary>
		public int Count
		{
			get
			{
				if (hideEmptyModels)
				{
					int count = 0;
					for (int i = 0; i < data.Count; i++)
						if (data[i].Exists)
							count++;
					return count;
				}
				else
				{
					return data.Count;
				}
			}
		}

		/// <summary>
		/// Gets the bitmap this instance represents.
		/// </summary>
		public FIBITMAP Dib
		{
			get
			{
				return dib;
			}
		}

		/// <summary>
		/// Gets or sets whether empty metadata models are hidden.
		/// </summary>
		public bool HideEmptyModels
		{
			get
			{
				return hideEmptyModels;
			}
			set
			{
				hideEmptyModels = value;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
		public IEnumerator GetEnumerator()
		{
			if (hideEmptyModels)
			{
				List<MetadataModel> tempList = new List<MetadataModel>(data.Count);
				for (int i = 0; i < data.Count; i++)
					if (data[i].Exists)
						tempList.Add(data[i]);
				return tempList.GetEnumerator();
			}
			else
			{
				return data.GetEnumerator();
			}
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
		public int CompareTo(object obj)
		{
			if (obj is ImageMetadata)
			{
				return CompareTo((ImageMetadata)obj);
			}
			throw new ArgumentException();
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this instance.</param>
		/// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
		public int CompareTo(ImageMetadata other)
		{
			return this.dib.CompareTo(other.dib);
		}
	}
}