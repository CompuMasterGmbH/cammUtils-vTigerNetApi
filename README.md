# cammUtils-vTigerNetApi

A library written in C# which allows easy access to the VTiger CRM webservice.
It is capable to use every aspect of the VTiger-Webservice as is. [![NuGet VTigerNetApi](https://img.shields.io/nuget/v/VTigerNetApi.svg?label=VTigerNetApi)](https://www.nuget.org/packages/VTigerNetApi/)

## Short example of using the VTiger .NET API

The following code is a brief example of how to use the VTiger .NET API to access the VTiger CRM database.

``` C#
// Create the VTiger object and set the destination URL for the VTiger-webservice
VTiger vtigerApi= new VTiger("http://localhost:8888");

// Process login with a username and it's matching authentication-key (see NOTE 1)
vtigerApi.Login("admin", "oQJ4I0h89gpir0zG");

// Query some elements (see NOTE 2)
DataTable dt = vtigerApi.Query("SELECT id, invoice_no FROM Invoice WHERE invoice_no='INV1';");

if (dt.Rows.Count > 0)
{
      // Retrieve all data of the entity from VTiger
      VTigerInvoice invoiceData = vtigerApi.Retrieve<VTigerInvoice>((string)dt.Rows[0]["id"]);

      // Display some of that data
      MessageBox.Show("Invoice INV1 has a grand-total of " + invoiceData.hdnGrandTotal.ToString());
}
```

### Notes

**NOTE 1** about VTiger-Login:
In order to log in, you need the authentication-key which can be found in user's profile-settings.

**NOTE 2** about VTiger-Queries:
Due to restrictions in the VTiger-webservice, neither "not" nor parenthesis work in queries.
If larger select-statements need to be constructed you can use the VTigerQueryWriter-class included in the project (recommended).

**NOTE 3** about VTiger-Update:
There is a problem caused by vtiger being unable to update some tables because they seem not to be indended to be updated. Nonetheless vtiger throws no error when trying to update those tables **resulting in corrupted entries**. For now we know that this is the case for the tables **quotes and sales-orders**. Because of that we highly recommend not updating that table and consider it as read-only.

### Some important information regarding VTiger-Queries
The VTiger-queries are NOT processed by SQL, but instead of a special parser inside VTiger and then rebuilt as SQL query for the actual VTiger database!
Because of that there are some kind of restrictions like the one described in NOTE 2.

### Trouble shooting
If you have any trouble building queries or you have problems regarding the data returned by VTiger you should first try using the VTigerManagerDemo and see what VTiger returnes and what queries can be executed.

## About
This project started as part of an internship and at last, this API would have never existed, if our trainee didn't get that chance by having an internship.

## Other useful links
* http://sourceforge.net/projects/vtigercrm/ The previous vtiger project website
* http://www.vtiger-crm.it/dotnetnuke/Progetti/NTigerNetAPI/tabid/68/Default.aspx Another .NET library for vtiger - let us know your feedback or recommendations!
