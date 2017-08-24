/*
Copyright 2011 Björn Zeutzheim

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.Data;
using System.Reflection;

namespace VTigerApi
{
    /// <summary>
    /// Client for the VTiger webservice API
    /// </summary>
    public partial class VTiger
    {
        private string serviceUrl;
        private string baseUrl;
        /// <summary>
        /// The URL of the VTiger-CRM (e.g. http://demo.vtiger.de/)
        /// </summary>
        public string ServiceUrl
        {
            get { return serviceUrl; }
            set
            {
                if (value[value.Length - 1] != '/')
                    value += "/";
                serviceUrl = value;
                baseUrl = value + "webservice.php?operation=";
            }
        }

        private string webserviceVersion;
        /// <summary>
        /// The version of the server's VTiger webservice to which the current user logged in
        /// </summary>
        public System.Version WebserviceVersion
        {
            get { return new System.Version(webserviceVersion); }
        }

        private System.Collections.Generic.Dictionary<string, TitleFields> remoteTables;
        /// <summary>
        /// Available tables at VTiger instance
        /// </summary>
        /// <remarks>
        /// Table list is only available when logged on
        /// </remarks>
        public System.Collections.Generic.Dictionary<string, TitleFields> RemoteTables
        {
            get {
                if (remoteTables != null)
                    return remoteTables;
                else
                    return new System.Collections.Generic.Dictionary<string, TitleFields>();
            }
        }

        private string vtigerVersion;
        /// <summary>
        /// The version of the server's VTiger software to which the current user logged in
        /// </summary>
        public System.Version VTigerVersion
        {
            get { return new System.Version(vtigerVersion); }
        }
        private string userID;
        /// <summary>
        /// The ID of the current VTiger user which is logged in
        /// </summary>
        public string UserID
        {
            get { return userID; }
        }

        private string sessionName;
        /// <summary>
        /// The session identifier which is used for authentication
        /// </summary>
        /// <remarks>
        /// This value is automatically set by login
        /// </remarks>
        /// <seealso cref="VTigerApi.VTiger.Login"/>
        public string SessionName
        {
            get { return sessionName; }
        }
        
        private Jayrock.Json.Conversion.ImportContext jsonImporter;
        private Jayrock.Json.Conversion.ExportContext jsonExporter;

        /// <summary>
        /// Create an instance of the VTiger-API interface
        /// </summary>
        /// <param name="aServiceUrl">Address of the service (example: http://demo.vtiger.de)</param>
        public VTiger(string aServiceUrl)
        {
            ServiceUrl = aServiceUrl;

            #region Json-Converters
            jsonImporter = new ImportContext();
            //JsonConvert.CurrentImportContextFactory = delegate { return jsonImporter; };
            jsonImporter.Register(new BooleanImporterEx());
            jsonImporter.Register(new DateTimeImporterEx());
            jsonImporter.Register(new Int32ImporterEx());
            jsonImporter.Register(new EmailAdressesImporter());
            jsonImporter.Register(new EnumValueImporter(typeof(TaskStatus), VTigerEnumValues.TaskstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Eventstatus), VTigerEnumValues.EventstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Taskpriority), VTigerEnumValues.TaskpriorityValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Activitytype), VTigerEnumValues.ActivitytypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Visibility), VTigerEnumValues.VisibilityValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Duration_minutes), VTigerEnumValues.Duration_minutesValues));
            jsonImporter.Register(new EnumValueImporter(typeof(RecurringType), VTigerEnumValues.RecurringtypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Leadsource), VTigerEnumValues.LeadsourceValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Industry), VTigerEnumValues.IndustryValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Leadstatus), VTigerEnumValues.LeadstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Rating), VTigerEnumValues.RatingValues));
            jsonImporter.Register(new EnumValueImporter(typeof(TaskStatus), VTigerEnumValues.TaskstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Email_flag), VTigerEnumValues.Email_flagValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Ticketpriorities), VTigerEnumValues.TicketprioritiesValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Ticketseverities), VTigerEnumValues.TicketseveritiesValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Ticketstatus), VTigerEnumValues.TicketstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Ticketcategories), VTigerEnumValues.TicketcategoriesValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Faqcategories), VTigerEnumValues.FaqcategoriesValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Faqstatus), VTigerEnumValues.FaqstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Quotestage), VTigerEnumValues.QuotestageValues));
            jsonImporter.Register(new EnumValueImporter(typeof(HdnTaxType), VTigerEnumValues.HdnTaxTypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(PoStatus), VTigerEnumValues.PostatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(SoStatus), VTigerEnumValues.SostatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Recurring_frequency), VTigerEnumValues.Recurring_frequencyValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Payment_duration), VTigerEnumValues.Payment_durationValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Invoicestatus), VTigerEnumValues.InvoicestatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Campaigntype), VTigerEnumValues.CampaigntypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Campaignstatus), VTigerEnumValues.CampaignstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Expectedresponse), VTigerEnumValues.ExpectedresponseValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Activity_view), VTigerEnumValues.Activity_viewValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Lead_view), VTigerEnumValues.Lead_viewValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Date_format), VTigerEnumValues.Date_formatValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Reminder_interval), VTigerEnumValues.Reminder_intervalValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Tracking_unit), VTigerEnumValues.Tracking_unitValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Contract_status), VTigerEnumValues.Contract_statusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Contract_priority), VTigerEnumValues.Contract_priorityValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Contract_type), VTigerEnumValues.Contract_typeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Service_usageunit), VTigerEnumValues.Service_usageunitValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Servicecategory), VTigerEnumValues.ServicecategoryValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Assetstatus), VTigerEnumValues.AssetstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projectmilestonetype), VTigerEnumValues.ProjectmilestonetypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projecttasktype), VTigerEnumValues.ProjecttasktypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projecttaskpriority), VTigerEnumValues.ProjecttaskpriorityValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projecttaskprogress), VTigerEnumValues.ProjecttaskprogressValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projectstatus), VTigerEnumValues.ProjectstatusValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projecttype), VTigerEnumValues.ProjecttypeValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Projectpriority), VTigerEnumValues.ProjectpriorityValues));
            jsonImporter.Register(new EnumValueImporter(typeof(Progress), VTigerEnumValues.ProgressValues));

            jsonExporter = JsonConvert.DefaultExportContextFactory();
            jsonExporter.Register(new BooleanExporterEx());
            jsonExporter.Register(new DateTimeExporterEx());
            jsonExporter.Register(new EmailAdressesExporter());
            jsonExporter.Register(new EnumValueExporter(typeof(TaskStatus), VTigerEnumValues.TaskstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Eventstatus), VTigerEnumValues.EventstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Taskpriority), VTigerEnumValues.TaskpriorityValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Activitytype), VTigerEnumValues.ActivitytypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Visibility), VTigerEnumValues.VisibilityValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Duration_minutes), VTigerEnumValues.Duration_minutesValues));
            jsonExporter.Register(new EnumValueExporter(typeof(RecurringType), VTigerEnumValues.RecurringtypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Leadsource), VTigerEnumValues.LeadsourceValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Industry), VTigerEnumValues.IndustryValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Leadstatus), VTigerEnumValues.LeadstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Rating), VTigerEnumValues.RatingValues));
            jsonExporter.Register(new EnumValueExporter(typeof(TaskStatus), VTigerEnumValues.TaskstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Email_flag), VTigerEnumValues.Email_flagValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Ticketpriorities), VTigerEnumValues.TicketprioritiesValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Ticketseverities), VTigerEnumValues.TicketseveritiesValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Ticketstatus), VTigerEnumValues.TicketstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Ticketcategories), VTigerEnumValues.TicketcategoriesValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Faqcategories), VTigerEnumValues.FaqcategoriesValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Faqstatus), VTigerEnumValues.FaqstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Quotestage), VTigerEnumValues.QuotestageValues));
            jsonExporter.Register(new EnumValueExporter(typeof(HdnTaxType), VTigerEnumValues.HdnTaxTypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(PoStatus), VTigerEnumValues.PostatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(SoStatus), VTigerEnumValues.SostatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Recurring_frequency), VTigerEnumValues.Recurring_frequencyValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Payment_duration), VTigerEnumValues.Payment_durationValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Invoicestatus), VTigerEnumValues.InvoicestatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Campaigntype), VTigerEnumValues.CampaigntypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Campaignstatus), VTigerEnumValues.CampaignstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Expectedresponse), VTigerEnumValues.ExpectedresponseValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Activity_view), VTigerEnumValues.Activity_viewValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Lead_view), VTigerEnumValues.Lead_viewValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Date_format), VTigerEnumValues.Date_formatValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Reminder_interval), VTigerEnumValues.Reminder_intervalValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Tracking_unit), VTigerEnumValues.Tracking_unitValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Contract_status), VTigerEnumValues.Contract_statusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Contract_priority), VTigerEnumValues.Contract_priorityValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Contract_type), VTigerEnumValues.Contract_typeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Service_usageunit), VTigerEnumValues.Service_usageunitValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Servicecategory), VTigerEnumValues.ServicecategoryValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Assetstatus), VTigerEnumValues.AssetstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projectmilestonetype), VTigerEnumValues.ProjectmilestonetypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projecttasktype), VTigerEnumValues.ProjecttasktypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projecttaskpriority), VTigerEnumValues.ProjecttaskpriorityValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projecttaskprogress), VTigerEnumValues.ProjecttaskprogressValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projectstatus), VTigerEnumValues.ProjectstatusValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projecttype), VTigerEnumValues.ProjecttypeValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Projectpriority), VTigerEnumValues.ProjectpriorityValues));
            jsonExporter.Register(new EnumValueExporter(typeof(Progress), VTigerEnumValues.ProgressValues));
            #endregion
        }

        #region Basic Access

        /// <summary>
        /// The typical title fields for a table row
        /// </summary>
        public class TitleFields
        {
            public string DefaultTitleField1;
            public string DefaultTitleField2;
            public VTigerType ElementType;
            public TitleFields()
            {
            }
            public TitleFields(string nameOfTitleField, VTigerType elementType)
            {
                DefaultTitleField1 = nameOfTitleField;
                DefaultTitleField2 = null;
                this.ElementType = elementType;
            }
            public TitleFields(string nameOfTitleField1, string nameOfTitleField2, VTigerType elementType)
            {
                DefaultTitleField1 = nameOfTitleField1;
                DefaultTitleField2 = nameOfTitleField2;
                this.ElementType = elementType;
            }
        }

        //====================================================================
        #region Login & Info

        private VTigerToken GetChallenge(string username)
        {
            return VTigerGetJson<VTigerToken>("getchallenge",
                String.Format("username={0}", username), false);
        }

        /// <summary>
        /// Login to the VTiger API.
        /// Neccessary to access any data.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="accessKey">Personal authentication-key provided on the VTiger website</param>
        public void Login(string username, string accessKey)
        {
            string token = GetChallenge(username).token;

            string key = getMd5Hash(token + accessKey);

            VTigerLogin loginResult = VTigerGetJson<VTigerLogin>("login",
                String.Format("username={0}&accessKey={1}", username, key), true);

            sessionName = loginResult.sessionName;
            vtigerVersion = loginResult.vtigerVersion;

            switch (vtigerVersion)
            {
                default:
                    {
                        System.Collections.Generic.Dictionary<string, TitleFields> nameFields = new System.Collections.Generic.Dictionary<string, TitleFields>();
                        nameFields.Add("Calendar", new TitleFields("subject", null, VTigerType.Calendar));
                        nameFields.Add("Leads", new TitleFields("firstname", "lastname", VTigerType.Leads));
                        nameFields.Add("Accounts", new TitleFields("accountname", null, VTigerType.Accounts));
                        nameFields.Add("Contacts", new TitleFields("firstname", "lastname", VTigerType.Contacts));
                        nameFields.Add("Potentials", new TitleFields("potentialname", null, VTigerType.Potentials));
                        nameFields.Add("Products", new TitleFields("productname", null, VTigerType.Products));
                        nameFields.Add("Documents", new TitleFields("notes_title", null, VTigerType.Documents));
                        nameFields.Add("Emails", new TitleFields("assigned_user_id", "subject", VTigerType.Emails));
                        nameFields.Add("HelpDesk", new TitleFields("ticket_title", null, VTigerType.HelpDesk));
                        nameFields.Add("Faq", new TitleFields("question", null, VTigerType.Faq));
                        nameFields.Add("Vendors", new TitleFields("vendorname", null, VTigerType.Vendors));
                        nameFields.Add("PriceBooks", new TitleFields("bookname", null, VTigerType.PriceBooks));
                        nameFields.Add("Quotes", new TitleFields("subject", null, VTigerType.Quotes));
                        nameFields.Add("PurchaseOrder", new TitleFields("subject", null, VTigerType.PurchaseOrder));
                        nameFields.Add("SalesOrder", new TitleFields("subject", null, VTigerType.SalesOrder));
                        nameFields.Add("Invoice", new TitleFields("subject", null, VTigerType.Invoice));
                        nameFields.Add("Campaigns", new TitleFields("campaignname", null, VTigerType.Campaigns));
                        nameFields.Add("Events", new TitleFields("subject", null, VTigerType.Events));
                        nameFields.Add("Users", new TitleFields("user_name", null, VTigerType.Users));
                        nameFields.Add("PBXManager", new TitleFields(null, null, VTigerType.PBXManager));
                        nameFields.Add("ServiceContracts", new TitleFields("subject", null, VTigerType.ServiceContracts));
                        nameFields.Add("Services", new TitleFields("servicename", null, VTigerType.Services));
                        nameFields.Add("Assets", new TitleFields("product", "assetname", VTigerType.Assets));
                        nameFields.Add("ModComments", new TitleFields("creator", "related_to", VTigerType.ModComments));
                        nameFields.Add("ProjectMilestone", new TitleFields("projectmilestonename", null, VTigerType.ProjectMilestone));
                        nameFields.Add("ProjectTask", new TitleFields("projecttaskname", null, VTigerType.ProjectTask));
                        nameFields.Add("Project", new TitleFields("projectname", null, VTigerType.Project));
                        nameFields.Add("SMSNotifier", new TitleFields(null, null, VTigerType.SMSNotifier));
                        nameFields.Add("Groups", new TitleFields("groupname", null, VTigerType.Groups));
                        nameFields.Add("Currency", new TitleFields("currency_name", null, VTigerType.Currency));
                        nameFields.Add("DocumentFolders", new TitleFields("foldername", null, VTigerType.DocumentFolders));
                        remoteTables = nameFields;
                        break;
                    }
            }

            webserviceVersion = loginResult.version;
            userID = loginResult.userId;
        }

        /// <summary>
        /// Terminates the current session
        /// </summary>
        public void Logout()
        {
            VTigerGetJson<JsonObject>("logout",
                String.Format("sessionName={0}", sessionName), false);
            sessionName = null;
            remoteTables = null;
        }

        /// <summary>
        /// Retrieve a list of the different entity-types supported by VTiger (for development)
        /// </summary>
        /// <returns></returns>
        public VTigerTypeInfo[] Listtypes()
        {
            VTigerTypes typeList = VTigerGetJson<VTigerTypes>("listtypes",
                String.Format("sessionName={0}", sessionName), false);

            typeList.typeInfo = new VTigerTypeInfo[typeList.types.Length];
            for (int i = 0; i < typeList.types.Length; i++)
            {
                string typeName = typeList.types[i];
                if (typeList.information.Contains(typeName))
                {
                    typeList.typeInfo[i] = ImportJson<VTigerTypeInfo>(typeList.information[typeName].ToString());
                    typeList.typeInfo[i].Name = typeName;
                }
            }
            return typeList.typeInfo;
        }

        /// <summary>
        /// Retrieve a list of the different entity-types supported by VTiger (for development)
        /// </summary>
        /// <returns></returns>
        public DataTable Listtypes_DataTable()
        {
            VTigerTypeInfo[] types = Listtypes();
            return JsonArrayToDataTable(ImportJson<JsonArray>(ExportJson(types)));
        }

        /// <summary>
        /// Retrieves detailed information about a VTiger entity-type (for development)
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public VTigerObjectType Describe(VTigerType elementType)
        {
            return VTigerGetJson<VTigerObjectType>("describe",
                String.Format("sessionName={0}&elementType={1}", sessionName, elementType), false);
        }

        /// <summary>
        /// Retrieves detailed information about a VTiger entity-type (for development)
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public DataTable Describe_DataTable(VTigerType elementType)
        {
            VTigerObjectType obj = Describe(elementType);
            return JsonArrayToDataTable(ImportJson<JsonArray>(ExportJson(obj.fields)));
        }

        #endregion

        //====================================================================
        #region Query & Retrieve

        /// <summary>
        /// Retrieve a single element with the specified id
        /// </summary>
        /// <typeparam name="T">Expected result-type (derivate of VTigerEntity)</typeparam>
        /// <param name="id">VTiger-ID</param>
        /// <returns></returns>
        public T Retrieve<T>(string id) //where T : JsonObject, VTigerEntity
        {
            return VTigerGetJson<T>("retrieve",
                String.Format("sessionName={0}&id={1}", sessionName, id), false);
        }

        /// <summary>
        /// Retrieve a single element with the specified id as a DataTable with a single row
        /// </summary>
        /// <param name="id">VTiger-ID</param>
        /// <returns></returns>
        public DataTable Retrieve(string id)
        {
            return JsonObjectToDataTable(Retrieve<JsonObject>(id));
        }

        /// <summary>
        /// Performs a query on the VTiger database
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public T VTiger_Query<T>(string query)
        {
            return VTigerGetJson<T>("query",
                String.Format("sessionName={0}&query={1}", sessionName, HttpUtility.UrlEncode(query)), false);
        }

        /// <summary>
        /// Performs a query on the VTiger database and converts the result to an array of the desired type
        /// </summary>
        /// <typeparam name="T">Expected entity-type</typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <example>
        /// This query will return the first 10 contacts whose firstname begins with an "M"  
        /// <code>Query&lt;VTigerContact&gt;("SELECT * FROM Contacts WHERE firstname LIKE 'M%' ORDER BY firstname LIMIT 0,10");</code></example>      
        public T[] Query<T>(string query) where T : VTigerEntity
        {
            //Query<VTigerContact>();  
            return VTiger_Query<T[]>(query);
        }

        /// <summary>
        /// Performs a query on the VTiger database and converts the result into a DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable Query(string query)
        {
            return JsonArrayToDataTable(VTiger_Query<JsonArray>(query));
        }

        #endregion

        //====================================================================
        #region Create

        /// <summary>
        /// Creates a new VTiger entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elementType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public T VTiger_Create<T>(VTigerType elementType, string element)
        {
            return VTigerGetJson<T>("create",
                String.Format("sessionName={0}&elementType={1}&element={2}", sessionName, elementType, HttpUtility.UrlEncode(element)), true);
        }

        /// <summary>
        /// Creates a new VTiger entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public T Create<T>(T element) where T : VTigerEntity
        {
            return VTiger_Create<T>(element.elementType, ExportJson(element));
        }

        /// <summary>
        /// Creates a new VTiger entity and return the result as a DataTable
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public DataTable Create(VTigerEntity element)
        {
            return JsonObjectToDataTable(VTiger_Create<JsonObject>(element.elementType, ExportJson(element)));
        }

        /// <summary>
        /// Creates a new VTiger entity and return the result as a DataTable
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public DataTable Create(VTigerType elementType, DataRow element)
        {
            return JsonObjectToDataTable(VTiger_Create<JsonObject>(elementType, ExportJson(element)));
        }

        /// <summary>
        /// Creates a new empty, locally stored VTiger entity
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public DataTable NewElement(VTigerType elementType)
        {
            Type dataType = VTigerTypeClasses[(int)elementType];
            DataTable dt = new DataTable();
            foreach (FieldInfo inf in dataType.GetFields())
                dt.Columns.Add(inf.Name, inf.FieldType);
            dt.Rows.Add(dt.NewRow());
            return dt;
        }
        /// <summary>
        /// Creates a new empty, locally stored VTiger entity with column scheme as provided by remote server
        /// </summary>
        /// <remarks>WARNING: the remote system must return at least 1 row! If the remote system returns 0 rows, there won't be any information on columns (table schema).</remarks>
        /// <param name="remoteTableName"></param>
        /// <returns></returns>
        public DataTable NewElementFromRemoteServerScheme(string remoteTableName)
        {
            string query = String.Format("select * from {0} limit 0;", remoteTableName);
            DataTable dt = this.Query(query);
            if (dt.Columns.Count == 0)
            {
                return this.NewElement(remoteTables[remoteTableName].ElementType);
            }
            else
            {
                dt.Rows.Add(dt.NewRow());
                return dt;
            }
        }

        #endregion

        //====================================================================
        #region Update

        /// <summary>
        /// Updates an existing entity in the VTiger database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        private T VTiger_Update<T>(string element)
        {
            return VTigerGetJson<T>("update",
                String.Format("sessionName={0}&element={1}", sessionName, HttpUtility.UrlEncode(element)), true);
        }

        /// <summary>
        /// Updates an existing entity in the VTiger database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public T Update<T>(T element) where T : VTigerEntity
        {
            return VTiger_Update<T>(ExportJson(element));
        }

        /// <summary>
        /// Updates an existing entity in the VTiger database
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public DataTable Update(DataRow element)
        {
            return JsonObjectToDataTable(VTiger_Update<JsonObject>(ExportJson(element)));
        }

        /// <summary>
        /// Fetches each entry from a DataTable and updates the corrosponding entities in the VTiger database
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public DataTable UpdateTable(DataTable elements)
        {
            if (elements.Rows.Count == 0)
                //return elements;
                throw new Exception("Could not perform update: Empty DataTable");

            DataTable result = elements.Clone();
            result.Clear();
            foreach (DataRow row in elements.Rows)
                result.ImportRow(Update(row).Rows[0]);
            return result;
        }

        #endregion

        //====================================================================
        #region Delete & Sync

        /// <summary>
        /// Delete an element from the database
        /// </summary>
        /// <param name="id">VTiger-ID</param>
        public void Delete(string id)
        {
            VTigerGetJson<object>("delete",
                String.Format("sessionName={0}&id={1}", sessionName, id), true);
        }

        public JsonObject Sync(DateTime modifiedTime)
        {
            int time = 0;
            JsonObject result;
            result = VTigerGetJson<JsonObject>("sync",
                String.Format("sessionName={0}&modifiedTime={1}", sessionName, time), false);
            return result;
        }

        public JsonObject Sync(DateTime modifiedTime, VTigerType elementType)
        {
            int time = 0;
            JsonObject result;
            result = VTigerGetJson<JsonObject>("sync",
                String.Format("sessionName={0}&modifiedTime={1}&elementType={2}", sessionName, time, elementType), false);
            return result;
        }

        #endregion

        //====================================================================
        #region Json-Conversion

        /// <summary>
        /// Exports an object with all accessible properties and fields as a Json-string
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public string ExportJson(object o)
        {
            using (JsonTextWriter writer = new JsonTextWriter())
            {
                jsonExporter.Export(o, writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Imports all possible properties and fields of a Json-string into a new instance of the desired class
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T ImportJson<T>(string json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                JsonTextReader reader = new JsonTextReader(stringReader);
                try
                {
                    return jsonImporter.Import<T>(reader);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " (" + json + ")");
                }
            }
        }

        /// <summary>
        /// Performs an operation with the VTiger webservice
        /// </summary>
        /// <param name="operation">operation-identifier</param>
        /// <param name="parameters">parameters for the operation</param>
        /// <param name="post">Specifies whether a HTTP-GET or HTTP-POST is used for the operation</param>
        /// <returns>VTiger response as string</returns>
        private string VTigerExecuteOperation(string operation, string parameters, bool post)
        {
            string response;
            if (post)
                response = HttpPost(baseUrl + operation, parameters);
            else
                response = HttpGet(baseUrl + operation + "&" + parameters);
            if (response == null)
                throw new Exception("Unable to connect to VTiger-Service");
            return response;
        }

        /// <summary>
        /// Performs an operation with the VTiger webservice and converts the result to the desired type
        /// </summary>
        /// <typeparam name="T">Expected type for the result</typeparam>
        /// <param name="operation">operation-identifier</param>
        /// <param name="parameters">parameters for the operation</param>
        /// <param name="post">Specifies whether a HTTP-GET or HTTP-POST is used for the operation</param>
        /// <returns></returns>
        private T VTigerGetJson<T>(string operation, string parameters, bool post)
        {
            string response = VTigerExecuteOperation(operation, parameters, post);
            VTigerResult<T> result = ImportJson<VTigerResult<T>>(response);
            if (!result.success)
            {
                if (result.error.code == "INVALID_SESSIONID")
                {
                    throw new VTigerApiSessionTimedOutException(result.error);
                }
                else
                { 
                    throw new VTigerApiException(result.error);
                }
            }
            return result.result;
        }

        /// <summary>
        /// Adds a new row to the DataTable and converts special fields if needed
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="obj"></param>
        private void DtConvertAddRow(DataTable dt, JsonObject obj)
        {
            DataRow dr = dt.NewRow();
            int i = 0;
            foreach (JsonMember member in obj)
            {
                if ((member.Name == "saved_toid") || (member.Name == "ccmail") || (member.Name == "bccmail"))
                {
                    if ((string)member.Value == "")
                        dr[i] = "";
                    else
                    {
                        string[] values = ImportJson<string[]>((string)member.Value);
                        dr[i] = ListStrings(values);
                    }
                }
                else
                    dr[i] = member.Value;
                i++;
            }
            dt.Rows.Add(dr);
        }

        /// <summary>
        /// Converts a JsonArray (from a query) into a DataTable
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public DataTable JsonArrayToDataTable(JsonArray array)
        {
            DataTable dt = new DataTable();
            if (array.Length == 0)
                return dt;

            object o = array[0];
            if (o is JsonObject)
            {
                JsonObject[] items = ImportJson<JsonObject[]>(array.ToString());

                foreach (JsonMember member in items[0])
                    dt.Columns.Add(new DataColumn(member.Name, typeof(string)));

                foreach (JsonObject item in items)
                    DtConvertAddRow(dt, item);

                if (dt.Columns.Contains("id"))
                    dt.Columns["id"].SetOrdinal(0);

                return dt;
            }
            throw new Exception("Only JsonArray of JsonObject can be deserialized to a DataTable");
        }

        /// <summary>
        /// Converts a JsonObject into a DataTable with a single row
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public DataTable JsonObjectToDataTable(JsonObject o)
        {
            DataTable dt = new DataTable();

            foreach (JsonMember member in o)
                dt.Columns.Add(new DataColumn(member.Name, typeof(string)));
            DtConvertAddRow(dt, o);

            if (dt.Columns.Contains("id"))
                dt.Columns["id"].SetOrdinal(0);

            return dt;
        }

        #endregion

        #endregion

        //====================================================================
        #region Searches

        private static string SqlContains = " LIKE '%{0}%'";
        private static string SqlDateFrom = " >= '{0}'";
        private static string SqlDateTo = " <= '{0}'";

        /// <summary>
        /// Creates a new VTigerQueryWriter and initializes it with default search-parameters.
        /// Empty parameters are excluded from the search.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="PrimaryCol">Primary search-column-name</param>
        /// <param name="OptionalCols">Optional search-column-names</param>
        /// <param name="DateCol">Column for date-search</param>
        /// <param name="PrimaryText"></param>
        /// <param name="OptionalText"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns>Returns the initialized VTigerQueryWriter</returns>       
        public VTigerQueryWriter DefaultSearchQuery(VTigerType table,
            string PrimaryCol, string PrimaryText,
            string[] OptionalCols, string OptionalText,
            string DateCol, DateTime FromDate, DateTime ToDate)
        {
            string optionalCmp = string.Format(" LIKE '%{0}%'", OptionalText.Replace(" ", "%"));
            string FromDateCmp = string.Format(" >= '{0}'", DateTimeToVtDate(FromDate));
            string ToDateCmp = string.Format(" <= '{0}'", DateTimeToVtDate(ToDate));

            VTigerQueryWriter queryWriter = new VTigerQueryWriter(table);

            if ((OptionalText != "") && (OptionalCols.Length != 0))
                foreach (string colName in OptionalCols)
                    queryWriter.AddOrCondition(colName + optionalCmp);

            if ((FromDate != DateTime.FromBinary(0)) && (DateCol != ""))
                queryWriter.AddAndCondition(DateCol + FromDateCmp);

            if ((ToDate != DateTime.FromBinary(0)) && (DateCol != ""))
                queryWriter.AddAndCondition(DateCol + ToDateCmp);

            if ((PrimaryText != "") && (PrimaryCol != ""))
                queryWriter.AddAndCondition(PrimaryCol + string.Format(SqlContains, PrimaryText));

            return queryWriter;
        }

        private string VTigerTableName (VTigerType elementType)
        {
            //return "vtiger_" + String.Format("{0}", elementType);
            return String.Format("{0}", elementType);
        }

        /// <summary>
        /// Searches for an entity which matches the specified condition and retrives it's ID
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="field">The field of the entity which should match the specified value</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FindEntityID(VTigerType elementType, string field, string value)
        {
            VTigerEntity[] items = Query<VTigerEntity>(String.Format(
                "SELECT id,{1} FROM {0} WHERE {1}='{2}';", VTigerTableName(elementType), field, value));
            if (items.Length == 0)
                throw new Exception(String.Format(
                    "Could not find an element for the condition {0}='{1}'", field, value));
            if (items.Length != 1)
                throw new Exception(String.Format(
                    "Found multiple elements with the condition {0}='{1}'", field, value));
            return items[0].id;
        }

        /// <summary>
        /// Searches for an entity which matches the specified condition and retrives it's data
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="field">The field of the entity which should match the specified value</param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// public T Create<T>(T element) where T : VTigerEntity
        public T FindEntity<T>(string field, string value) where T : VTigerEntity, new()
        {
            T[] items = Query<T>(String.Format(
                "SELECT * FROM {0} WHERE {1}='{2}';", VTigerTableName((new T()).elementType), field, value));
            if (items.Length == 0)
                return null;
                //throw new Exception(String.Format(
                //    "Could not find an element for the condition {0}='{1}'", field, value));
            if (items.Length != 1)
                throw new Exception(String.Format(
                    "Found multiple elements with the condition {0}='{1}'", field, value));
            return items[0];
        }

        #region Default GetID-functions

        public string GetUserID(string name)
        {
            return FindEntityID(VTigerType.Users, "user_name", name);
        }

        public string GetAccountID(string name)
        {
            return FindEntityID(VTigerType.Accounts, "accountname", name);
        }

        public string GetProductID(string name)
        {
            return FindEntityID(VTigerType.Products, "productname", name);
        }

        public string GetCampaignID(string name)
        {
            return FindEntityID(VTigerType.Campaigns, "campaignname", name);
        }

        public string GetServiceID(string name)
        {
            return FindEntityID(VTigerType.Services, "servicename", name);
        }

        public string GetAssetID(string name)
        {
            return FindEntityID(VTigerType.Assets, "assetname", name);
        }

        public string GetProjectTaskID(string name)
        {
            return FindEntityID(VTigerType.ProjectTask, "projecttaskname", name);
        }

        public string GetProjectID(string name)
        {
            return FindEntityID(VTigerType.Project, "projectname", name);
        }

        public string GetGroupID(string name)
        {
            return FindEntityID(VTigerType.Products, "groupname", name);
        }

        public string GetCurrencyID(string name)
        {
            return FindEntityID(VTigerType.Products, "currency_name", name);
        }

        #endregion

        #region Default Searches

        /// <summary>
        /// Returns a default search-query
        /// </summary>
        /// <remarks>
        /// Default search-attributes:
        /// Primary-Column: invoice_no
        /// Optional-Columns: subject, hdnGrandTotal, hdnSubTotal, hdnDiscountAmount, txtAdjustment, terms_conditions
        /// Date-Column: invoicedate
        /// </remarks>
        /// <param name="invoice_no"></param>
        /// <param name="searchText"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        /// <seealso cref="VTigerApi.VTiger.DefaultSearchQuery"/>
        public VTigerQueryWriter DefaultSearchInvoices(string invoice_no, string searchText, DateTime fromDate, DateTime toDate)
        {
            return DefaultSearchQuery(VTigerType.Invoice, "invoice_no", invoice_no,
                new string[] { "subject", "hdnGrandTotal", "hdnSubTotal", "hdnDiscountAmount", "txtAdjustment", "terms_conditions" }, searchText,
                "invoicedate", fromDate, toDate);
        }

        #endregion

        #endregion

        //====================================================================

        #region Default Add-functions

        public VTigerCalendar AddCalendar(string user_id, string subject,
            DateTime date_start, DateTime due_date, TaskStatus taskStatus)
        {
            //Todo: For some kind of reason the server returns success, without creating a new element
            VTigerCalendar element = new VTigerCalendar(
                subject,
                user_id,
                DateTimeToVtDate(date_start),
                DateTimeToVtDate(due_date),
                taskStatus);
            return Create<VTigerCalendar>(element);
        }

        public VTigerLead AddLead(string lastname, string company, string assigned_user_id)
        {
            VTigerLead element = new VTigerLead(
                lastname,
                company,
                assigned_user_id);
            return Create<VTigerLead>(element);
        }

        public VTigerAccount AddAccount(string accountname, string assigned_user_id)
        {
            VTigerAccount element = new VTigerAccount(
                accountname,
                assigned_user_id);
            return Create<VTigerAccount>(element);
        }

        public VTigerContact AddContact(string firstname, string lastname, string user_id)
        {
            VTigerContact element = new VTigerContact(
                lastname,
                user_id);
            element.firstname = firstname;
            //element.
                return Update<VTigerContact>(element);
            return Create<VTigerContact>(element);
        }

        public VTigerPotential AddPotential(string potentialname, string related_to,
            string closingdate, Sales_stage sales_stage, string assigned_user_id)
        {
            VTigerPotential element = new VTigerPotential(
                potentialname,
                related_to,
                closingdate,
                sales_stage,
                assigned_user_id);
            return Create<VTigerPotential>(element);
        }

        public VTigerProduct AddProduct(string productname)
        {
            return Create<VTigerProduct>(new VTigerProduct(productname));
        }

        public VTigerDocument AddDocument(string notes_title, string assigned_user_id)
        {
            VTigerDocument element = new VTigerDocument(
                notes_title,
                assigned_user_id);
            return Create<VTigerDocument>(element);
        }

        public VTigerEmail AddEmail(string subject, DateTime date_start,
            string from_email, string[] saved_toid, string assigned_user_id)
        {
            VTigerEmail element = new VTigerEmail(
                subject,
                date_start,
                from_email,
                saved_toid,
                assigned_user_id);
            return Create<VTigerEmail>(element);
        }

        public VTigerHelpDesk AddHelpDesk(string assigned_user_id, Ticketstatus ticketstatus, string ticket_title)
        {
            VTigerHelpDesk element = new VTigerHelpDesk(
                assigned_user_id,
                ticketstatus,
                ticket_title);
            return Create<VTigerHelpDesk>(element);
        }

        public VTigerFaq AddFaq(Faqstatus faqstatus, string question, string faq_answer)
        {
            VTigerFaq element = new VTigerFaq(
                faqstatus,
                question,
                faq_answer);
            return Create<VTigerFaq>(element);
        }

        public VTigerVendor AddVendor(string vendorname)
        {
            return Create<VTigerVendor>(new VTigerVendor(vendorname));
        }

        public VTigerPriceBook AddPriceBook(string bookname, string currency_id)
        {
            VTigerPriceBook element = new VTigerPriceBook(
                bookname,
                currency_id);
            return Create<VTigerPriceBook>(element);
        }

        public VTigerQuote AddQuote(string subject, Quotestage quotestage, string bill_street,
            string ship_street, string account_id, string assigned_user_id)
        {
            VTigerQuote element = new VTigerQuote(
                subject,
                quotestage,
                bill_street,
                ship_street,
                account_id,
                assigned_user_id);
            return Create<VTigerQuote>(element);
        }

        public VTigerPurchaseOrder AddPurchaseOrder(string subject, string vendor_id, PoStatus postatus,
            string bill_street, string ship_street, string assigned_user_id)
        {
            VTigerPurchaseOrder element = new VTigerPurchaseOrder(
                subject,
                vendor_id,
                postatus,
                bill_street,
                ship_street,
                assigned_user_id);
            return Create<VTigerPurchaseOrder>(element);
        }

        public VTigerSalesOrder AddSalesOrder(string subject, SoStatus sostatus, string bill_street,
            string ship_street, Invoicestatus invoicestatus, string account_id, string assigned_user_id)
        {
            VTigerSalesOrder element = new VTigerSalesOrder(
                subject,
                sostatus,
                bill_street,
                ship_street,
                invoicestatus,
                account_id,
                assigned_user_id);
            return Create<VTigerSalesOrder>(element);
        }

        public VTigerInvoice AddInvoice(string subject, string bill_street, string ship_street,
            string account_id, string assigned_user_id)
        {
            VTigerInvoice element = new VTigerInvoice(
                subject,
                bill_street,
                ship_street,
                account_id,
                assigned_user_id);
            return Create<VTigerInvoice>(element);
        }

        public VTigerCampaign AddCampaign(string campaignname, DateTime closingdate, string assigned_user_id)
        {
            VTigerCampaign element = new VTigerCampaign(
                campaignname,
                closingdate,
                assigned_user_id);
            return Create<VTigerCampaign>(element);
        }

        public VTigerEvent AddEvent(string subject, string date_start, string time_start, string due_date,
            string time_end, int duration_hours, Eventstatus eventstatus,
            Activitytype activitytype, string assigned_user_id)
        {
            VTigerEvent element = new VTigerEvent(
                subject,
                date_start,
                time_start,
                due_date,
                time_end,
                duration_hours,
                eventstatus,
                activitytype,
                assigned_user_id);
            return Create<VTigerEvent>(element);
        }

        public VTigerPBXManager AddPBXManager(string callfrom, string callto)
        {
            VTigerPBXManager element = new VTigerPBXManager(
                callfrom,
                callto);
            return Create<VTigerPBXManager>(element);
        }

        public VTigerServiceContract AddServiceContract(string subject, string assigned_user_id)
        {
            VTigerServiceContract element = new VTigerServiceContract(
                subject,
                assigned_user_id);
            return Create<VTigerServiceContract>(element);
        }

        public VTigerService AddService(string servicename)
        {
            return Create<VTigerService>(new VTigerService(servicename));
        }

        public VTigerAsset AddAsset(string product, string serialnumber, string datesold,
            string dateinservice, Assetstatus assetstatus, string assetname,
            string account, string assigned_user_id)
        {
            VTigerAsset element = new VTigerAsset(
                product,
                serialnumber,
                datesold,
                dateinservice,
                assetstatus,
                assetname,
                account,
                assigned_user_id);
            return Create<VTigerAsset>(element);
        }

        //public VTigerDocument Add()
        //{
        //    VTigerDocument element = new VTigerDocument(
        //        account_id,
        //        assigned_user_id);
        //    return Create<VTigerDocument>(element);
        //}

        #endregion

        //====================================================================
        #region Helpers

        private static string getMd5Hash(string input)
        {
            if ((input == null) || (input.Length == 0))
            {
                return string.Empty;
            }
            byte[] data;
            using (MD5 md5Hasher = MD5.Create())
                data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static string HttpGet(string url)
        {
            HttpWebRequest webRequest = GetWebRequest(url);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string jsonResponse = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = sr.ReadToEnd();
            }
            return jsonResponse;
        }

        private static string HttpPost(string url, string parameters)
        {
            HttpWebRequest webRequest = GetWebRequest(url);
            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.Method = "POST";
            byte[] data = Encoding.ASCII.GetBytes(parameters);
            webRequest.ContentLength = data.Length;
            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            { // get the response
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse == null)
                { return null; }
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException)
            {
                throw;
                //MessageBox.Show(ex.Message, "HttpPost: Response error",
                //   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static HttpWebRequest GetWebRequest(string formattedUri)
        {
            // Create the request’s URI.      
            Uri serviceUri = new Uri(formattedUri, UriKind.Absolute);
            // Return the HttpWebRequest.        
            return (HttpWebRequest)System.Net.WebRequest.Create(serviceUri);
        }

        public static VTigerType VTigerTypeParse(string text)
        {
            return (VTigerType)Enum.Parse(typeof(VTigerType), text, true);
        }

        public static string DateTimeToVtDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string DateTimeToVtTime(DateTime time)
        {
            return time.ToString("hh:mm:ss");
        }

        public string ListStrings(string[] strings)
        {
            if (strings.Length == 0)
                return "";
            if (strings.Length == 1)
                return strings[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strings.Length - 1; i++)
                sb.Append(strings[i] + ",");
            sb.Append(strings[strings.Length - 1]);
            return sb.ToString();
        }

        #endregion
    }

    public class VTigerQueryWriter
    {
        public string[] columns = { "*" };
        public VTigerType table;
        public int limitStart = 0;
        public int limitCount = 100;
        public string[][] conditions;// = new string[0][];

        public VTigerQueryWriter(VTigerType aTable)
        {
            table = aTable;
        }

        public override string ToString()
        {
            return Query;
        }

        public string Query
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ");
                if (columns.Length > 0)
                {
                    foreach (string item in columns)
                        sb.Append(item + ", ");
                    sb.Remove(sb.Length - 2, 2);
                    sb.AppendLine();
                }
                else
                    sb.AppendLine("*");

                sb.Append("FROM ");
                sb.AppendLine(table.ToString());

                if ((conditions != null) && (conditions.Length > 0))
                {
                    sb.Append("WHERE ");
                    foreach (string[] orCond in conditions)
                    {
                        foreach (string andCond in orCond)
                        {
                            sb.Append(andCond);
                            sb.Append(" AND ");
                        }
                        sb.Remove(sb.Length - 5, 5);
                        sb.Append(" OR ");
                    }
                    sb.Remove(sb.Length - 4, 4);
                    sb.AppendLine();
                }
                sb.Append("LIMIT ");
                sb.Append(limitStart.ToString());
                sb.Append(", ");
                sb.Append(limitCount.ToString());

                sb.Append(";");
                return sb.ToString();
            }
        }

        public void AddAndCondition(string condition)
        {
            if ((conditions == null) || (conditions.Length == 0))
            {
                conditions = new string[1][];
                conditions[0] = new string[1];
                conditions[0][0] = condition;
            }
            else
            {
                for (int i = 0; i < conditions.Length; i++)
                {
                    string[] newList = new string[conditions[i].Length + 1];
                    for (int j = 0; j < conditions[i].Length; j++)
                        newList[j] = conditions[i][j];
                    newList[newList.Length - 1] = condition;
                    conditions[i] = newList;
                }
            }
        }

        public void AddOrCondition(string condition)
        {
            if ((conditions == null) || (conditions.Length == 0))
            {
                conditions = new string[1][];
                conditions[0] = new string[1];
                conditions[0][0] = condition;
            }
            else
            {
                string[][] newList = new string[conditions.Length + 1][];
                for (int i = 0; i < conditions.Length; i++)
                {
                    newList[i] = conditions[i];
                }
                newList[conditions.Length] = new string[1];
                newList[conditions.Length][0] = condition;
                conditions = newList;
            }
        }
    }
}