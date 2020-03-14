﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class GUI_SecurityRecordsEntriesPage : NetPage
{
	[SerializeField]
	private EmptyItemList recordsList = null;
	private GUI_SecurityRecords securityRecordsTab;
	private List<SecurityRecord> currentRecords = new List<SecurityRecord>();
	[SerializeField]
	private NetLabel idNameText = null;

	public void OnOpen(GUI_SecurityRecords recordsTab)
	{
		securityRecordsTab = recordsTab;
		ResetList();
		UpdateTab();
	}

	/// <summary>
	/// Reseting list - removing all search conditions.
	/// </summary>
	private void ResetList()
	{
		List<SecurityRecord> records = SecurityRecordsManager.Instance.SecurityRecords;
		
		currentRecords.Clear();
		for (int i = 0; i < records.Count; i++)
		{
			currentRecords.Add(records[i]);
		}
	}

/// <summary>
/// Searches for records containing specific text.
/// All ToLower() might be slow, but otherwise search would be case-sensitive
/// </summary>
/// <param name="searchText">Text to search</param>
	public void Search(string searchText)
	{
		List<SecurityRecord> newList = new List<SecurityRecord>();
		ResetList();

		if (!searchText.IsNullOrEmpty())
		{
			searchText = searchText.ToLower();
			foreach (var record in currentRecords)
			{
				if (record.EntryName.ToLower().Contains(searchText) || record.Age.ToLower().Contains(searchText) ||
					record.ID.ToLower().Contains(searchText) || record.Rank.ToLower().Contains(searchText) ||
					record.Sex.ToLower().Contains(searchText) || record.Species.ToLower().Contains(searchText) ||
					record.Fingerprints.ToLower().Contains(searchText) ||
					record.Status.ToString().ToLower().Contains(searchText))
				{
					newList.Add(record);
				}
			}
			currentRecords.Clear();
			currentRecords.AddRange(newList);
		}
		UpdateTab();
	}

	public void RemoveID()
	{
		securityRecordsTab.RemoveId();
		securityRecordsTab.UpdateIdText(idNameText);
	}

	public void UpdateTab()
	{
		if (!CustomNetworkManager.Instance._isServer)
		{
			return;
		}

		securityRecordsTab.UpdateIdText(idNameText);
		recordsList.Clear();
		recordsList.AddItems(currentRecords.Count);
		for (int i = 0; i < currentRecords.Count; i++)
		{
			GUI_SecurityRecordsItem item = recordsList.Entries[i] as GUI_SecurityRecordsItem;
			item.ReInit(currentRecords[i], securityRecordsTab);
			item.gameObject.SetActive(true);
		}
	}

	public void NewRecord()
	{
		List<SecurityRecord> records = SecurityRecordsManager.Instance.SecurityRecords;
		SecurityRecord record = new SecurityRecord();
		records.Add(record);
		ResetList();
		UpdateTab();
	}
}
