//_______________________________________________________________
//  Title   : Dot net nuke data set
//  System  : Microsoft VisualStudio 2015 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2016, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;
using System.Data;

namespace CAS.DNNDataBase.UsersManagement
{

  /// <summary>
  /// Class DatabaseDNNDataSet representing DNN database 
  /// </summary>
  /// <seealso cref="System.Data.DataSet" />
  partial class DatabaseDNNDataSet
  {
    partial class DNNRegisteredUsersRow : DataRow
    {
      /// <summary>
      /// Determines whether the user is from the specified country.
      /// </summary>
      /// <param name="country">The country.</param>
      /// <param name="anyOther">if set to <c>true</c> select the user if it is not from the selected country.</param>
      /// <returns><c>true</c> if the user is from the expected country; otherwise, <c>false</c>.</returns>
      internal bool IsUserFrom(string country, bool anyOther)
      {
        bool _match = false;
        if (this.IsEmailNull())
          return false;
        if (this.IsPropertyValueNull() && country != "")
          return false;
        if ((this.IsPropertyValueNull() && country == "") ||
          (this.PropertyValue.ToLower().Contains(country.ToLower()) && !anyOther) ||
          (!this.PropertyValue.ToLower().Contains(country.ToLower()) && anyOther)
          )
          _match = true;
        return _match;
      }
    }
    partial class dnn1_OnyakNEOptInsDataTable
    {
      /// <summary>
      /// Unsubscribe the user by email from all campaigns.
      /// </summary>
      /// <param name="email">The email.</param>
      /// <returns><code>true</code> if the email matches and the account is enabled; <code>false</code> otherwise</returns>
      internal bool UnsubscribeUserByEmailFromAllCampaigns(string email)
      {
        bool _unsubscribe = false;
        foreach (dnn1_OnyakNEOptInsRow row in this)
        {
          if (row.Email.ToLower().Contains(email.ToLower()) && row.Enabled)
          {
            row.Enabled = false;
            row.DisabledDate = System.DateTime.Now;
            _unsubscribe = true;
          }
        }
        return _unsubscribe;
      }
      /// <summary>
      /// Updates or adds entry to the specified campaign.
      /// </summary>
      /// <param name="campaignID">The unique campaign ID.</param>
      /// <param name="email">The email of the user.</param>
      /// <param name="firstName">The first name.</param>
      /// <param name="lastName">The last name.</param>
      internal void UpdateOrAddEntry(int campaignID, string email, string firstName, string lastName)
      {
        bool _newRow = false;
        dnn1_OnyakNEOptInsRow _optInsRow = null;
        foreach (dnn1_OnyakNEOptInsRow row in this)
        {
          if (row.CampaignSystemID == campaignID && row.Email == email)
          {
            _optInsRow = row;
            break;
          }
        }
        if (_optInsRow == null)
        {
          _optInsRow = this.Newdnn1_OnyakNEOptInsRow();
          _optInsRow.CampaignSystemID = campaignID;
          _optInsRow.Email = email;
          _optInsRow.Enabled = true;
          _optInsRow.CreatedDate = DateTime.Now;
          _optInsRow.DisabledDate = new DateTime(1900, 1, 1);
          _newRow = true;
        }
        if (firstName != null)
          _optInsRow.FirstName = firstName;
        if (lastName != null)
          _optInsRow.LastName = lastName;
        if (_newRow)
          this.Adddnn1_OnyakNEOptInsRow(_optInsRow);
      }
    }
    partial class dnn1_OnyakNECampaignDataTable
    {
      /// <summary>
      /// Finds the identifier of a selected campaign.
      /// </summary>
      /// <param name="campaign">The campaign name.</param>
      /// <returns>The campaign identifier if exist;otherwise -1 </returns>
      internal int FindId(string campaign)
      {
        int ret = -1;
        foreach (dnn1_OnyakNECampaignRow row in this)
        {
          if (row.Name == campaign)
          {
            ret = row.CampaignSystemID;
            break;
          }
        }
        return ret;
      }
    }

  }
}

