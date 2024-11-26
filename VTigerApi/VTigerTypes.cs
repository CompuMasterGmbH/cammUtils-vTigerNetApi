/*
Copyright 2011, 2017 CompuMaster GmbH
Authors: Björn Zeutzheim + Jochen Wezel

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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VTigerApi
{
    #region enums

    public enum TaskStatus { Not_Started, In_Progress, Completed, Pending_Input, Deferred, Planned }
    public enum Eventstatus { Planned, Held, Not_Held }
    public enum Taskpriority { High, Medium, Low }
    public enum Activitytype { Call, Meeting }
    public enum Visibility { Private, Public }
    public enum Duration_minutes { _00, _15, _30, _45 }
    public enum RecurringType { None, Daily, Weekly, Monthly, Yearly }
    public enum Leadsource
    {
        None, Cold_Call, Existing_Customer, Self_Generated,
        Employee, Partner, Public_Relations, Direct_Mail,
        Conference, Trade_Show, Web_Site, Word_of_mouth, Other
    }
    public enum Industry
    {
        None, Apparel, Banking, Biotechnology, Chemicals,
        Communications, Construction, Consulting, Education,
        Electronics, Energy, Engineering, Entertainment,
        Environmental, Finance, Food_Beverage, Government,
        Healthcare, Hospitality, Insurance, Machinery,
        Manufacturing, Media, Not_For_Profit, Recreation,
        Retail, Shipping, Technology, Telecommunications,
        Transportation, Utilities, Other
    }
    public enum Leadstatus
    {
        None, Attempted_to_Contact, Cold, Contact_in_Future,
        Contacted, Hot, Junk_Lead, Lost_Lead, Not_Contacted,
        Pre_Qualified, Qualified, Warm
    }
    public enum Rating { None, Acquired, Active, Market_Failed, Project_Cancelled, Shutdown }
    public enum Accounttype { None, Analyst, Competitor, Customer, Integrator, Investor, Partner, Press, Prospect, Reseller, Other }
    public enum Opportunity_type { None, Existing_Business, New_Business }
    public enum Sales_stage { Prospecting, Qualification, Needs_Analysis, Value_Proposition, Id_Decision_Makers, Perception_Analysis, Proposal_Price_Quote, Negotiation_Review, Closed_Won, Closed_Lost }
    public enum Email_flag { SAVED, SENT }
    public enum Ticketpriorities { Low, Normal, High, Urgent }
    public enum Ticketseverities { Minor, Major, Feature, Critical }
    public enum Ticketstatus { Open, In_Progress, Wait_For_Response, Closed }
    public enum Ticketcategories { Big_Problem, Small_Problem, Other_Problem }
    public enum Faqcategories { General }
    public enum Faqstatus { Draft, Reviewed, Published, Obsolete }
    public enum Quotestage { Created, Delivered, Reviewed, Accepted, Rejected }
    public enum HdnTaxType { Individual, Group }
    public enum PoStatus { Created, Approved, Delivered, Cancelled, Received_Shipment }
    public enum SoStatus { Created, Approved, Delivered, Cancelled }
    public enum Recurring_frequency { None, Daily, Weekly, Monthly, Quarterly, Yearly }
    public enum Payment_duration { Net_30_days, Net_45_days, Net_60_days }
    public enum Invoicestatus { AutoCreated, Created, Approved, Sent, Credit_Invoice, Paid }
    public enum Campaigntype { None, Conference, Webinar, Trade_Show, Public_Relations, Partners, Referral_Program, Advertisement, Banner_Ads, Direct_Mail, Email, Telemarketing, Others }
    public enum Campaignstatus { None, Planning, Active, Inactive, Completed, Cancelled }
    public enum Expectedresponse { None, Excellent, Good, Average, Poor }
    public enum Activity_view { Today, This_Week, This_Month, This_Year }
    public enum Lead_view { Today, Last_2_Days, Last_Week }
    public enum Date_format { ddmmyyyy, mmddyyyy, yyyymmdd }
    public enum Reminder_interval { None, _1_Minute, _5_Minutes, _15_Minutes, _30_Minutes, _45_Minutes, _1_Hour, _1_Day }
    public enum Tracking_unit { None, Hours, Days, Incidents }
    public enum Contract_status { Undefined, In_Planning, In_Progress, On_Hold, Complete, Archived }
    public enum Contract_priority { Low, Normal, High }
    public enum Contract_type { Support, Services, Administrative }
    public enum Service_usageunit { Hours, Days, Incidents }
    public enum Servicecategory { None, Support, Installation, Migration, Customization, Training }
    public enum Assetstatus { In_Service, Outofservice }
    public enum Projectmilestonetype { none, administrative, operative, other }
    public enum Projecttasktype { none, administrative, operative, other }
    public enum Projecttaskpriority { none, low, normal, high }
    public enum Projecttaskprogress { none, _10_Percent, _20_Percent, _30_Percent, _40_Percent, _50_Percent, _60_Percent, _70_Percent, _80_Percent, _90_Percent, _100_Percent }
    public enum Projectstatus { none, Prospecting, Initiated, In_Progress, waiting_for_feedback, On_Hold, Completed, Delivered, Archived }
    public enum Projecttype { none, administrative, operative, other }
    public enum Projectpriority { none, low, normal, high }
    public enum Progress { none, _10_Percent, _20_Percent, _30_Percent, _40_Percent, _50_Percent, _60_Percent, _70_Percent, _80_Percent, _90_Percent, _100_Percent }

    #endregion

    public struct EmailAdresses
    {
        public string[] Adresses;
    }

    //====================================================================

    /// <summary>
    /// A session timeout at the server may require re-login
    /// </summary>
    public class VTigerApiSessionTimedOutException : VTigerApiException
    {
        internal VTigerApiSessionTimedOutException(VTigerError apiError) : base(apiError)
        {
            base.VTigerErrorCode = apiError.Code;
            base.VTigerMessage = apiError.Message;
        }
    }

    /// <summary>
    /// An exception as reported by the remote server
    /// </summary>
    public class VTigerApiException : Exception
    {
        internal VTigerApiException(VTigerError apiError)
        {
            this.VTigerErrorCode = apiError.Code;
            this.VTigerMessage = apiError.Message;
        }

        /// <summary>
        /// The error code as defined by the VTiger remote server
        /// </summary>
        public string VTigerErrorCode;

        /// <summary>
        /// A human-readable error message from the VTiger remote server
        /// </summary>
        public string VTigerMessage;

        /// <summary>
        /// The full message from the VTiger remote server (error code + human-readable message)
        /// </summary>
        public override string Message =>
            string.IsNullOrEmpty(VTigerErrorCode)
                ? $"UNKNOWN ERROR: {VTigerMessage}"
                : $"{VTigerErrorCode}: {VTigerMessage}";
    }

    //====================================================================
    #region VTiger-Access-Classes

    /// <summary>
    /// Represents the result from a VTiger API call
    /// </summary>
    public class VTigerResult<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public VTigerError Error { get; set; }

        /// <summary>
        /// The result value
        /// </summary>
        [JsonPropertyName("result")]
        public T Result { get; set; }
    }

    /// <summary>
    /// VTiger error information
    /// </summary>
    public class VTigerError
    {
        /// <summary>
        /// An error code from the vtiger API
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// An error message from the vtiger API
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class VTigerLogin
    {
        [JsonPropertyName("sessionName")]
        public string SessionName { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("vtigerVersion")]
        public string VTigerVersion { get; set; }
    }

    public class VTigerToken
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("serverTime")]
        public int ServerTime { get; set; }

        [JsonPropertyName("expireTime")]
        public int ExpireTime { get; set; }
    }

    public class VTigerTypes
    {
        [JsonPropertyName("types")]
        public string[] Types { get; set; }

        [JsonPropertyName("typeInfo")]
        public VTigerTypeInfo[] TypeInfo { get; set; }

        [JsonPropertyName("information")]
        public JsonDocument Information { get; set; } // JsonObject wurde durch JsonDocument ersetzt.
    }

    public class VTigerTypeInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("singular")]
        public string Singular { get; set; }

        [JsonPropertyName("isEntity")]
        public bool IsEntity { get; set; }
    }

    //====================================================================

    /// <summary>
    /// Contains the description of a VTiger-object
    /// </summary>
    public class VTigerObjectType
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("createable")]
        public bool Createable { get; set; }

        [JsonPropertyName("updateable")]
        public bool Updateable { get; set; }

        [JsonPropertyName("deleteable")]
        public bool Deleteable { get; set; }

        [JsonPropertyName("retrieveable")]
        public bool Retrieveable { get; set; }

        [JsonPropertyName("fields")]
        public VTigerObjectField[] Fields { get; set; }

        [JsonPropertyName("idPrefix")]
        public string IdPrefix { get; set; }

        [JsonPropertyName("isEntity")]
        public string IsEntity { get; set; }

        [JsonPropertyName("labelFields")]
        public string LabelFields { get; set; }
    }

    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerObjectField
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("mandatory")]
        public bool Mandatory { get; set; }

        [JsonPropertyName("type")]
        public VTigerTypeDesc Type { get; set; }

        [JsonPropertyName("nullable")]
        public bool Nullable { get; set; }

        [JsonPropertyName("editable")]
        public bool Editable { get; set; }
    }

    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerTypeDesc
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picklistValues")]
        public VTigerPicklistItem[] PicklistValues { get; set; }

        [JsonPropertyName("refersTo")]
        public string[] RefersTo { get; set; }
    }

    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerPicklistItem
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    #endregion

    public class VTigerEnumValues
    {
        public static string[] TaskstatusValues = { "Not Started", "In Progress", "Completed", "Pending Input", "Deferred", "Planned" };
        public static string[] EventstatusValues = { "Planned", "Held", "Not Held" };
        public static string[] TaskpriorityValues = { "High", "Medium", "Low" };
        public static string[] ActivitytypeValues = { "Call", "Meeting" };
        public static string[] VisibilityValues = { "Private", "Public" };
        public static string[] Duration_minutesValues = { "00", "15", "30", "45" };
        public static string[] RecurringtypeValues = { "--None--", "Daily", "Weekly", "Monthly", "Yearly" };
        public static string[] LeadsourceValues = { "--None--", "Cold Call", "Existing Customer", "Self Generated", "Employee", "Partner", "Public Relations", "Direct Mail", "Conference", "Trade Show", "Web Site", "Word of mouth", "Other" };
        public static string[] IndustryValues = { "--None--", "Apparel", "Banking", "Biotechnology", "Chemicals", "Communications", "Construction", "Consulting", "Education", "Electronics", "Energy", "Engineering", "Entertainment", "Environmental", "Finance", "Food & Beverage", "Government", "Healthcare", "Hospitality", "Insurance", "Machinery", "Manufacturing", "Media", "Not For Profit", "Recreation", "Retail", "Shipping", "Technology", "Telecommunications", "Transportation", "Utilities", "Other" };
        public static string[] LeadstatusValues = { "--None--", "Attempted to Contact", "Cold", "Contact in Future", "Contacted", "Hot", "Junk Lead", "Lost Lead", "Not Contacted", "Pre Qualified", "Qualified", "Warm" };
        public static string[] RatingValues = { "--None--", "Acquired", "Active", "Market Failed", "Project Cancelled", "Shutdown" };
        public static string[] AccounttypeValues = { "--None--", "Analyst", "Competitor", "Customer", "Integrator", "Investor", "Partner", "Press", "Prospect", "Reseller", "Other" };
        public static string[] Opportunity_typeValues = { "--None--", "Existing Business", "New Business" };
        public static string[] Sales_stageValues = { "Prospecting", "Qualification", "Needs Analysis", "Value Proposition", "Id. Decision Makers", "Perception Analysis", "Proposal/Price Quote", "Negotiation/Review", "Closed Won", "Closed Lost" };
        public static string[] Email_flagValues = { "SAVED", "SENT" };
        public static string[] TicketprioritiesValues = { "Low", "Normal", "High", "Urgent" };
        public static string[] TicketseveritiesValues = { "Minor", "Major", "Feature", "Critical" };
        public static string[] TicketstatusValues = { "Open", "In Progress", "Wait For Response", "Closed" };
        public static string[] TicketcategoriesValues = { "Big Problem", "Small Problem", "Other Problem" };
        public static string[] FaqcategoriesValues = { "General" };
        public static string[] FaqstatusValues = { "Draft", "Reviewed", "Published", "Obsolete" };
        public static string[] QuotestageValues = { "Created", "Delivered", "Reviewed", "Accepted", "Rejected" };
        public static string[] HdnTaxTypeValues = { "individual", "group" };
        public static string[] PostatusValues = { "Created", "Approved", "Delivered", "Cancelled", "Received Shipment" };
        public static string[] SostatusValues = { "Created", "Approved", "Delivered", "Cancelled" };
        public static string[] Recurring_frequencyValues = { "--None--", "Daily", "Weekly", "Monthly", "Quarterly", "Yearly" };
        public static string[] Payment_durationValues = { "Net 30 days", "Net 45 days", "Net 60 days" };
        public static string[] InvoicestatusValues = { "AutoCreated", "Created", "Approved", "Sent", "Credit Invoice", "Paid" };
        public static string[] CampaigntypeValues = { "--None--", "Conference", "Webinar", "Trade Show", "Public Relations", "Partners", "Referral Program", "Advertisement", "Banner Ads", "Direct Mail", "Email", "Telemarketing", "Others" };
        public static string[] CampaignstatusValues = { "--None--", "Planning", "Active", "Inactive", "Completed", "Cancelled" };
        public static string[] ExpectedresponseValues = { "--None--", "Excellent", "Good", "Average", "Poor" };
        public static string[] Activity_viewValues = { "Today", "This Week", "This Month", "This Year" };
        public static string[] Lead_viewValues = { "Today", "Last 2 Days", "Last Week" };
        public static string[] Date_formatValues = { "dd-mm-yyyy", "mm-dd-yyyy", "yyyy-mm-dd" };
        public static string[] Reminder_intervalValues = { "None", "1 Minute", "5 Minutes", "15 Minutes", "30 Minutes", "45 Minutes", "1 Hour", "1 Day" };
        public static string[] Tracking_unitValues = { "None", "Hours", "Days", "Incidents" };
        public static string[] Contract_statusValues = { "Undefined", "In Planning", "In Progress", "On Hold", "Complete", "Archived" };
        public static string[] Contract_priorityValues = { "Low", "Normal", "High" };
        public static string[] Contract_typeValues = { "Support", "Services", "Administrative" };
        public static string[] Service_usageunitValues = { "Hours", "Days", "Incidents" };
        public static string[] ServicecategoryValues = { "--None--", "Support", "Installation", "Migration", "Customization", "Training" };
        public static string[] AssetstatusValues = { "In Service", "Out-of-service" };
        public static string[] ProjectmilestonetypeValues = { "--none--", "administrative", "operative", "other" };
        public static string[] ProjecttasktypeValues = { "--none--", "administrative", "operative", "other" };
        public static string[] ProjecttaskpriorityValues = { "--none--", "low", "normal", "high" };
        public static string[] ProjecttaskprogressValues = { "--none--", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
        public static string[] ProjectstatusValues = { "--none--", "prospecting", "initiated", "in progress", "waiting for feedback", "on hold", "completed", "delivered", "archived" };
        public static string[] ProjecttypeValues = { "--none--", "administrative", "operative", "other" };
        public static string[] ProjectpriorityValues = { "--none--", "low", "normal", "high" };
        public static string[] ProgressValues = { "--none--", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };
    }

    public enum VTigerType
    {
        Undefined = 0,
        Calendar = 1, Leads = 2, Accounts = 3,
        Contacts = 4, Potentials = 5, Products = 6,
        Documents = 7, Emails = 8, HelpDesk = 9,
        Faq = 10, Vendors = 11, PriceBooks = 12,
        Quotes = 13, PurchaseOrder = 14, SalesOrder = 15,
        Invoice = 16, Campaigns = 17, Events = 18,
        Users = 19, PBXManager = 20, ServiceContracts = 21,
        Services = 22, Assets = 23, ModComments = 24,
        ProjectMilestone = 25, ProjectTask = 26,
        Project = 27, SMSNotifier = 28, Groups = 29,
        Currency = 30, DocumentFolders = 31
    }

    public partial class VTiger
    {
        public static Type[] VTigerTypeClasses =
        {
            typeof(VTigerEntity),
            typeof(VTigerCalendar), typeof(VTigerLead), typeof(VTigerAccount),
            typeof(VTigerContact), typeof(VTigerPotential), typeof(VTigerProduct),
            typeof(VTigerDocument), typeof(VTigerEmail), typeof(VTigerHelpDesk),
            typeof(VTigerFaq), typeof(VTigerVendor), typeof(VTigerPriceBook),
            typeof(VTigerQuote), typeof(VTigerPurchaseOrder), typeof(VTigerSalesOrder),
            typeof(VTigerInvoice), typeof(VTigerCampaign), typeof(VTigerEvent),
            typeof(VTigerUser), typeof(VTigerPBXManager), typeof(VTigerServiceContract),
            typeof(VTigerService), typeof(VTigerAsset), typeof(VTigerModComment),
            typeof(VTigerProjectMilestone), typeof(VTigerProjectTask),
            typeof(VTigerProject), typeof(VTigerSMSNotifier), typeof(VTigerGroup),
            typeof(VTigerCurrency), typeof(VTigerDocumentFolder)
        };
    }

    #region Element-Classes

    public class VTigerEntity
    {
        [JsonIgnore]
        public VTigerType ElementType => GetElementType();

        public virtual VTigerType GetElementType()
        {
            return VTigerType.Undefined;
        }

        public virtual string RemoteTableName()
        {
            return null;
        }

        public virtual VTigerEntity CreateNewInstance()
        {
            return new VTigerEntity();
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// VTiger-Calendar object
    /// </summary>
    public class VTigerCalendar : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerCalendar();
        }

        public override string RemoteTableName() { return "Calendar"; }

        public override VTigerType GetElementType() { return VTigerType.Calendar; }

        public VTigerCalendar() { }

        public VTigerCalendar(string subject, string assignedUserId, string dateStart, string timeStart, string dueDate, TaskStatus taskStatus)
        {
            this.Subject = subject;
            this.AssignedUserId = assignedUserId;
            this.DateStart = dateStart;
            this.DueDate = dueDate;
            this.TaskStatus = taskStatus;
            this.TimeStart = timeStart;
        }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; } // mandatory

        [JsonPropertyName("time_start")]
        public string TimeStart { get; set; }

        [JsonPropertyName("time_end")]
        public string TimeEnd { get; set; }

        [JsonPropertyName("due_date")]
        public string DueDate { get; set; } // mandatory

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("taskstatus")]
        public TaskStatus TaskStatus { get; set; } // mandatory

        [JsonPropertyName("eventstatus")]
        public Eventstatus EventStatus { get; set; }

        [JsonPropertyName("taskpriority")]
        public Taskpriority TaskPriority { get; set; }

        [JsonPropertyName("sendnotification")]
        public bool SendNotification { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("activitytype")]
        public Activitytype ActivityType { get; set; }

        [JsonPropertyName("visibility")]
        public Visibility Visibility { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("duration_hours")]
        public string DurationHours { get; set; }

        [JsonPropertyName("duration_minutes")]
        public Duration_minutes DurationMinutes { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("reminder_time")]
        public int ReminderTime { get; set; }

        [JsonPropertyName("recurringtype")]
        public RecurringType RecurringType { get; set; }

        [JsonPropertyName("notime")]
        public bool NoTime { get; set; }
    }

    /// <summary>
    /// VTiger-Leads object
    /// </summary>
    public class VTigerLead : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerLead();
        }

        public override string RemoteTableName() { return "Leads"; }

        public override VTigerType GetElementType() { return VTigerType.Leads; }

        public VTigerLead() { }

        public VTigerLead(string lastName, string company, string assignedUserId)
        {
            this.LastName = lastName;
            this.Company = company;
            this.AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("salutationtype")]
        public string SalutationType { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }

        [JsonPropertyName("lead_no")]
        public string LeadNo { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("lastname")]
        public string LastName { get; set; } // mandatory

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("company")]
        public string Company { get; set; } // mandatory

        [JsonPropertyName("fax")]
        public string Fax { get; set; }

        [JsonPropertyName("designation")]
        public string Designation { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("leadsource")]
        public Leadsource LeadSource { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("industry")]
        public Industry Industry { get; set; }

        [JsonPropertyName("leadstatus")]
        public Leadstatus LeadStatus { get; set; }

        [JsonPropertyName("annualrevenue")]
        public int AnnualRevenue { get; set; }

        [JsonPropertyName("rating")]
        public Rating Rating { get; set; }

        [JsonPropertyName("noofemployees")]
        public int NoOfEmployees { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("yahooid")]
        public string YahooId { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("lane")]
        public string Lane { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("pobox")]
        public string POBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Accounts object
    /// </summary>
    public class VTigerAccount : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerAccount();
        }

        public override string RemoteTableName() { return "Accounts"; }

        public override VTigerType GetElementType() { return VTigerType.Accounts; }

        public VTigerAccount() { }

        public VTigerAccount(string accountname, string assigned_user_id)
        {
            this.AccountName = accountname;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("accountname")]
        public string AccountName { get; set; } // mandatory

        [JsonPropertyName("account_no")]
        public string AccountNo { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("fax")]
        public string Fax { get; set; }

        [JsonPropertyName("tickersymbol")]
        public string TickerSymbol { get; set; }

        [JsonPropertyName("otherphone")]
        public string OtherPhone { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("email1")]
        public string Email1 { get; set; }

        [JsonPropertyName("employees")]
        public int Employees { get; set; }

        [JsonPropertyName("email2")]
        public string Email2 { get; set; }

        [JsonPropertyName("ownership")]
        public string Ownership { get; set; }

        [JsonPropertyName("rating")]
        public Rating Rating { get; set; }

        [JsonPropertyName("industry")]
        public Industry Industry { get; set; }

        [JsonPropertyName("siccode")]
        public string SicCode { get; set; }

        [JsonPropertyName("accounttype")]
        public Accounttype AccountType { get; set; }

        [JsonPropertyName("annual_revenue")]
        public int AnnualRevenue { get; set; }

        [JsonPropertyName("emailoptout")]
        public bool EmailOptOut { get; set; }

        [JsonPropertyName("notify_owner")]
        public bool NotifyOwner { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("bill_street")]
        public string BillStreet { get; set; }

        [JsonPropertyName("ship_street")]
        public string ShipStreet { get; set; }

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("ship_city")]
        public string ShipCity { get; set; }

        [JsonPropertyName("bill_state")]
        public string BillState { get; set; }

        [JsonPropertyName("ship_state")]
        public string ShipState { get; set; }

        [JsonPropertyName("bill_code")]
        public string BillCode { get; set; }

        [JsonPropertyName("ship_code")]
        public string ShipCode { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("ship_country")]
        public string ShipCountry { get; set; }

        [JsonPropertyName("bill_pobox")]
        public string BillPOBox { get; set; }

        [JsonPropertyName("ship_pobox")]
        public string ShipPOBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("isconvertedfromlead")]
        public string IsConvertedFromLead { get; set; }
    }

    /// <summary>
    /// VTiger-Contacts object
    /// </summary>
    public class VTigerContact : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerContact();
        }

        public override string RemoteTableName() { return "Contacts"; }

        public override VTigerType GetElementType() { return VTigerType.Contacts; }

        public VTigerContact() { }

        public VTigerContact(string lastname, string assigned_user_id)
        {
            this.LastName = lastname;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("salutationtype")]
        public string SalutationType { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }

        [JsonPropertyName("contact_no")]
        public string ContactNo { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("lastname")]
        public string LastName { get; set; } // mandatory

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        [JsonPropertyName("homephone")]
        public string HomePhone { get; set; }

        [JsonPropertyName("leadsource")]
        public Leadsource LeadSource { get; set; }

        [JsonPropertyName("otherphone")]
        public string OtherPhone { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("fax")]
        public string Fax { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("birthday")]
        public string Birthday { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("assistant")]
        public string Assistant { get; set; }

        [JsonPropertyName("yahooid")]
        public string YahooId { get; set; }

        [JsonPropertyName("assistantphone")]
        public string AssistantPhone { get; set; }

        [JsonPropertyName("donotcall")]
        public bool DoNotCall { get; set; }

        [JsonPropertyName("emailoptout")]
        public bool EmailOptOut { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("reference")]
        public bool Reference { get; set; }

        [JsonPropertyName("notify_owner")]
        public bool NotifyOwner { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("portal")]
        public bool Portal { get; set; }

        [JsonPropertyName("support_start_date")]
        public string SupportStartDate { get; set; }

        [JsonPropertyName("support_end_date")]
        public string SupportEndDate { get; set; }

        [JsonPropertyName("mailingstreet")]
        public string MailingStreet { get; set; }

        [JsonPropertyName("otherstreet")]
        public string OtherStreet { get; set; }

        [JsonPropertyName("mailingcity")]
        public string MailingCity { get; set; }

        [JsonPropertyName("othercity")]
        public string OtherCity { get; set; }

        [JsonPropertyName("mailingstate")]
        public string MailingState { get; set; }

        [JsonPropertyName("otherstate")]
        public string OtherState { get; set; }

        [JsonPropertyName("mailingzip")]
        public string MailingZip { get; set; }

        [JsonPropertyName("otherzip")]
        public string OtherZip { get; set; }

        [JsonPropertyName("mailingcountry")]
        public string MailingCountry { get; set; }

        [JsonPropertyName("othercountry")]
        public string OtherCountry { get; set; }

        [JsonPropertyName("mailingpobox")]
        public string MailingPOBox { get; set; }

        [JsonPropertyName("otherpobox")]
        public string OtherPOBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("secondaryemail")]
        public string SecondaryEmail { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("imagename")]
        public string ImageName { get; set; }

        [JsonPropertyName("isconvertedfromlead")]
        public string IsConvertedFromLead { get; set; }
    }

    /// <summary>
    /// VTiger-Potentials object
    /// </summary>
    public class VTigerPotential : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerPotential();
        }

        public override string RemoteTableName() { return "Potentials"; }

        public override VTigerType GetElementType() { return VTigerType.Potentials; }

        public VTigerPotential() { }

        public VTigerPotential(string potentialname, string related_to, string closingdate, Sales_stage sales_stage, string assigned_user_id)
        {
            this.PotentialName = potentialname;
            this.RelatedTo = related_to;
            this.ClosingDate = closingdate;
            this.SalesStage = sales_stage;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("potentialname")]
        public string PotentialName { get; set; } // mandatory

        [JsonPropertyName("potential_no")]
        public string PotentialNo { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("related_to")]
        public string RelatedTo { get; set; } // mandatory

        [JsonPropertyName("closingdate")]
        public string ClosingDate { get; set; } // mandatory

        [JsonPropertyName("opportunity_type")]
        public Opportunity_type OpportunityType { get; set; }

        [JsonPropertyName("nextstep")]
        public string NextStep { get; set; }

        [JsonPropertyName("leadsource")]
        public Leadsource LeadSource { get; set; }

        [JsonPropertyName("sales_stage")]
        public Sales_stage SalesStage { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("probability")]
        public double Probability { get; set; }

        [JsonPropertyName("campaignid")]
        public string CampaignId { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Products object
    /// </summary>
    public class VTigerProduct : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerProduct();
        }

        public override string RemoteTableName() { return "Products"; }

        public override VTigerType GetElementType() { return VTigerType.Products; }

        public VTigerProduct() { }

        public VTigerProduct(string productname, string assigned_user_id)
        {
            this.ProductName = productname;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("productname")]
        public string ProductName { get; set; } // mandatory

        [JsonPropertyName("product_no")]
        public string ProductNo { get; set; }

        [JsonPropertyName("productcode")]
        public string ProductCode { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("productcategory")]
        public string ProductCategory { get; set; }

        [JsonPropertyName("sales_start_date")]
        public string SalesStartDate { get; set; }

        [JsonPropertyName("sales_end_date")]
        public string SalesEndDate { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("vendor_id")]
        public string VendorId { get; set; }

        [JsonPropertyName("mfr_part_no")]
        public string MfrPartNo { get; set; }

        [JsonPropertyName("vendor_part_no")]
        public string VendorPartNo { get; set; }

        [JsonPropertyName("serial_no")]
        public string SerialNo { get; set; }

        [JsonPropertyName("productsheet")]
        public string ProductSheet { get; set; }

        [JsonPropertyName("glacct")]
        public string GlAcct { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("unit_price")]
        public double UnitPrice { get; set; }

        [JsonPropertyName("commissionrate")]
        public double CommissionRate { get; set; }

        [JsonPropertyName("taxclass")]
        public string TaxClass { get; set; }

        [JsonPropertyName("usageunit")]
        public string UsageUnit { get; set; }

        [JsonPropertyName("qty_per_unit")]
        public double QtyPerUnit { get; set; }

        [JsonPropertyName("qtyinstock")]
        public double QtyInStock { get; set; }

        [JsonPropertyName("reorderlevel")]
        public int ReorderLevel { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("qtyindemand")]
        public int QtyInDemand { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("imagename")]
        public string ImageName { get; set; }

        [JsonPropertyName("purchase_cost")]
        public string PurchaseCost { get; set; }
    }

    /// <summary>
    /// VTiger-Documents object
    /// </summary>
    public class VTigerDocument : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerDocument();
        }

        public override string RemoteTableName() { return "Documents"; }

        public override VTigerType GetElementType() { return VTigerType.Documents; }

        public VTigerDocument() { }

        public VTigerDocument(string notes_title, string assigned_user_id)
        {
            this.NotesTitle = notes_title;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("notes_title")]
        public string NotesTitle { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("notecontent")]
        public string NoteContent { get; set; }

        [JsonPropertyName("filetype")]
        public string FileType { get; set; }

        [JsonPropertyName("filesize")]
        public int FileSize { get; set; }

        [JsonPropertyName("filelocationtype")]
        public string FileLocationType { get; set; }

        [JsonPropertyName("fileversion")]
        public string FileVersion { get; set; }

        [JsonPropertyName("filestatus")]
        public bool FileStatus { get; set; }

        [JsonPropertyName("filedownloadcount")]
        public int FileDownloadCount { get; set; }

        [JsonPropertyName("folderid")]
        public string FolderId { get; set; }

        [JsonPropertyName("note_no")]
        public string NoteNo { get; set; }
    }

    /// <summary>
    /// VTiger-Emails object
    /// </summary>
    public class VTigerEmail : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerEmail();
        }

        public override string RemoteTableName() { return "Emails"; }

        public override VTigerType GetElementType() { return VTigerType.Emails; }

        public VTigerEmail() { }

        public VTigerEmail(string subject, DateTime date_start, string from_email, string[] saved_toid, string assigned_user_id)
        {
            this.DateStart = date_start.ToString("yyyy-MM-dd HH:mm:ss");
            this.AssignedUserId = assigned_user_id;
            this.Subject = subject;
            this.FromEmail = from_email;
            this.SavedToId = new EmailAdresses { Adresses = saved_toid };
            this.ActivityType = "Emails";
        }

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; } // mandatory

        [JsonIgnore]
        public string ParentType { get; set; } // Read-only

        [JsonPropertyName("activitytype")]
        public string ActivityType { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("time_start")]
        public string TimeStart { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("access_count")]
        public string AccessCount { get; set; }

        [JsonPropertyName("from_email")]
        public string FromEmail { get; set; } // mandatory

        [JsonPropertyName("saved_toid")]
        public EmailAdresses SavedToId { get; set; } // mandatory

        [JsonPropertyName("ccmail")]
        public EmailAdresses CcMail { get; set; }

        [JsonPropertyName("bccmail")]
        public EmailAdresses BccMail { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("email_flag")]
        public Email_flag EmailFlag { get; set; }
    }

    /// <summary>
    /// VTiger-HelpDesk object
    /// </summary>
    public class VTigerHelpDesk : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerHelpDesk();
        }

        public override string RemoteTableName() { return "HelpDesk"; }

        public override VTigerType GetElementType() { return VTigerType.HelpDesk; }

        public VTigerHelpDesk() { }

        public VTigerHelpDesk(string assigned_user_id, Ticketstatus ticketstatus, string ticket_title)
        {
            this.AssignedUserId = assigned_user_id;
            this.TicketStatus = ticketstatus;
            this.TicketTitle = ticket_title;
        }

        [JsonPropertyName("ticket_no")]
        public string TicketNo { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("ticketpriorities")]
        public Ticketpriorities TicketPriorities { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("ticketseverities")]
        public Ticketseverities TicketSeverities { get; set; }

        [JsonPropertyName("ticketstatus")]
        public Ticketstatus TicketStatus { get; set; } // mandatory

        [JsonPropertyName("ticketcategories")]
        public Ticketcategories TicketCategories { get; set; }

        [JsonPropertyName("update_log")]
        public string UpdateLog { get; set; }

        [JsonPropertyName("hours")]
        public int Hours { get; set; }

        [JsonPropertyName("days")]
        public int Days { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("ticket_title")]
        public string TicketTitle { get; set; } // mandatory

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("solution")]
        public string Solution { get; set; }
    }

    /// <summary>
    /// VTiger-Faq object
    /// </summary>
    public class VTigerFaq : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerFaq();
        }

        public override string RemoteTableName() { return "Faq"; }

        public override VTigerType GetElementType() { return VTigerType.Faq; }

        public VTigerFaq() { }

        public VTigerFaq(Faqstatus faqstatus, string question, string faq_answer)
        {
            this.FaqStatus = faqstatus;
            this.Question = question;
            this.FaqAnswer = faq_answer;
        }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("faq_no")]
        public string FaqNo { get; set; }

        [JsonPropertyName("faqcategories")]
        public Faqcategories FaqCategories { get; set; }

        [JsonPropertyName("faqstatus")]
        public Faqstatus FaqStatus { get; set; } // mandatory

        [JsonPropertyName("question")]
        public string Question { get; set; } // mandatory

        [JsonPropertyName("faq_answer")]
        public string FaqAnswer { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }
    }

    /// <summary>
    /// VTiger-Vendors object
    /// </summary>
    public class VTigerVendor : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerVendor();
        }

        public override string RemoteTableName() { return "Vendors"; }

        public override VTigerType GetElementType() { return VTigerType.Vendors; }

        public VTigerVendor() { }

        public VTigerVendor(string vendorname, string assigned_user_id)
        {
            this.VendorName = vendorname;
            this.AssignedUserId = assigned_user_id;
        }

        [JsonPropertyName("vendorname")]
        public string VendorName { get; set; } // mandatory

        [JsonPropertyName("vendor_no")]
        public string VendorNo { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("glacct")]
        public string GlAcct { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("pobox")]
        public string PoBox { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("postalcode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; }
    }

    /// <summary>
    /// VTiger-PriceBooks object
    /// </summary>
    public class VTigerPriceBook : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerPriceBook();
        }

        public override string RemoteTableName() { return "PriceBooks"; }

        public override VTigerType GetElementType() { return VTigerType.PriceBooks; }

        public VTigerPriceBook() { }

        public VTigerPriceBook(string bookname, string currency_id)
        {
            this.BookName = bookname;
            this.CurrencyId = currency_id;
        }

        [JsonPropertyName("bookname")]
        public string BookName { get; set; } // mandatory

        [JsonPropertyName("pricebook_no")]
        public string PriceBookNo { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; } // mandatory

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Quotes object
    /// </summary>
    public class VTigerQuote : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerQuote();
        }

        public override string RemoteTableName() { return "Quotes"; }

        public override VTigerType GetElementType() { return VTigerType.Quotes; }

        public VTigerQuote() { }

        public VTigerQuote(string subject, Quotestage quotestage, string bill_street,
            string ship_street, string account_id, string assigned_user_id)
        {
            this.Subject = subject;
            this.QuoteStage = quotestage;
            this.AccountId = account_id;
            this.AssignedUserId = assigned_user_id;
            this.BillStreet = bill_street;
            this.ShipStreet = ship_street;
        }

        [JsonPropertyName("quote_no")]
        public string QuoteNo { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("potential_id")]
        public string PotentialId { get; set; }

        [JsonPropertyName("quotestage")]
        public Quotestage QuoteStage { get; set; } // mandatory

        [JsonPropertyName("validtill")]
        public string ValidTill { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("hdnSubTotal")]
        public double HiddenSubTotal { get; set; }

        [JsonPropertyName("shipping")]
        public string Shipping { get; set; }

        [JsonPropertyName("assigned_user_id1")]
        public string AssignedUserId1 { get; set; }

        [JsonPropertyName("txtAdjustment")]
        public double TextAdjustment { get; set; }

        [JsonPropertyName("hdnGrandTotal")]
        public double HiddenGrandTotal { get; set; }

        [JsonPropertyName("hdnTaxType")]
        public HdnTaxType HiddenTaxType { get; set; }

        [JsonPropertyName("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonPropertyName("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonPropertyName("hdnDiscountPercent")]
        public double HiddenDiscountPercent { get; set; }

        [JsonPropertyName("hdnDiscountAmount")]
        public double HiddenDiscountAmount { get; set; }

        [JsonPropertyName("hdnS_H_Amount")]
        public double HiddenShippingHandlingAmount { get; set; }

        [JsonPropertyName("hdnS_H_Percent")]
        public double HiddenShippingHandlingPercent { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("conversion_rate")]
        public double ConversionRate { get; set; }

        [JsonPropertyName("bill_street")]
        public string BillStreet { get; set; } // mandatory

        [JsonPropertyName("ship_street")]
        public string ShipStreet { get; set; } // mandatory

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("ship_city")]
        public string ShipCity { get; set; }

        [JsonPropertyName("bill_state")]
        public string BillState { get; set; }

        [JsonPropertyName("ship_state")]
        public string ShipState { get; set; }

        [JsonPropertyName("bill_code")]
        public string BillCode { get; set; }

        [JsonPropertyName("ship_code")]
        public string ShipCode { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("ship_country")]
        public string ShipCountry { get; set; }

        [JsonPropertyName("bill_pobox")]
        public string BillPoBox { get; set; }

        [JsonPropertyName("ship_pobox")]
        public string ShipPoBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("terms_conditions")]
        public string TermsConditions { get; set; }

        [JsonPropertyName("productid")]
        public string ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        [JsonPropertyName("listprice")]
        public double ListPrice { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tax1")]
        public string Tax1 { get; set; }

        [JsonPropertyName("tax2")]
        public string Tax2 { get; set; }

        [JsonPropertyName("tax3")]
        public string Tax3 { get; set; }

        [JsonPropertyName("pre_tax_total")]
        public double PreTaxTotal { get; set; }
    }

    /// <summary>
    /// VTiger-PurchaseOrder object
    /// </summary>
    public class VTigerPurchaseOrder : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerPurchaseOrder();
        }

        public override string RemoteTableName() { return "PurchaseOrder"; }

        public override VTigerType GetElementType() { return VTigerType.PurchaseOrder; }

        public VTigerPurchaseOrder() { }

        public VTigerPurchaseOrder(string subject, string vendorId, PoStatus poStatus,
            string billStreet, string shipStreet, string assignedUserId)
        {
            Subject = subject;
            VendorId = vendorId;
            PoStatus = poStatus;
            AssignedUserId = assignedUserId;
            BillStreet = billStreet;
            ShipStreet = shipStreet;
        }

        [JsonPropertyName("purchaseorder_no")]
        public string PurchaseOrderNo { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("vendor_id")]
        public string VendorId { get; set; } // mandatory

        [JsonPropertyName("requisition_no")]
        public string RequisitionNo { get; set; }

        [JsonPropertyName("tracking_no")]
        public string TrackingNo { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("duedate")]
        public string DueDate { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("txtAdjustment")]
        public double TxtAdjustment { get; set; }

        [JsonPropertyName("salescommission")]
        public double SalesCommission { get; set; }

        [JsonPropertyName("exciseduty")]
        public double ExciseDuty { get; set; }

        [JsonPropertyName("hdnGrandTotal")]
        public double HiddenGrandTotal { get; set; }

        [JsonPropertyName("hdnSubTotal")]
        public double HiddenSubTotal { get; set; }

        [JsonPropertyName("hdnTaxType")]
        public HdnTaxType HiddenTaxType { get; set; }

        [JsonPropertyName("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonPropertyName("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonPropertyName("hdnDiscountPercent")]
        public double HiddenDiscountPercent { get; set; }

        [JsonPropertyName("hdnDiscountAmount")]
        public double HiddenDiscountAmount { get; set; }

        [JsonPropertyName("hdnS_H_Amount")]
        public double HiddenShippingHandlingAmount { get; set; }

        [JsonPropertyName("hdnS_H_Percent")]
        public double HiddenShippingHandlingPercent { get; set; }

        [JsonPropertyName("postatus")]
        public PoStatus PoStatus { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("conversion_rate")]
        public double ConversionRate { get; set; }

        [JsonPropertyName("bill_street")]
        public string BillStreet { get; set; } // mandatory

        [JsonPropertyName("ship_street")]
        public string ShipStreet { get; set; } // mandatory

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("ship_city")]
        public string ShipCity { get; set; }

        [JsonPropertyName("bill_state")]
        public string BillState { get; set; }

        [JsonPropertyName("ship_state")]
        public string ShipState { get; set; }

        [JsonPropertyName("bill_code")]
        public string BillCode { get; set; }

        [JsonPropertyName("ship_code")]
        public string ShipCode { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("ship_country")]
        public string ShipCountry { get; set; }

        [JsonPropertyName("bill_pobox")]
        public string BillPoBox { get; set; }

        [JsonPropertyName("ship_pobox")]
        public string ShipPoBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("terms_conditions")]
        public string TermsConditions { get; set; }

        [JsonPropertyName("productid")]
        public string ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        [JsonPropertyName("listprice")]
        public double ListPrice { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tax1")]
        public string Tax1 { get; set; }

        [JsonPropertyName("tax2")]
        public string Tax2 { get; set; }

        [JsonPropertyName("tax3")]
        public string Tax3 { get; set; }

        [JsonPropertyName("pre_tax_total")]
        public double PreTaxTotal { get; set; }

        [JsonPropertyName("paid")]
        public double Paid { get; set; }

        [JsonPropertyName("balance")]
        public double Balance { get; set; }
    }

    /// <summary>
    /// VTiger-SalesOrder object
    /// </summary>
    public class VTigerSalesOrder : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerSalesOrder();
        }

        public override string RemoteTableName() { return "SalesOrder"; }

        public override VTigerType GetElementType() { return VTigerType.SalesOrder; }

        public VTigerSalesOrder() { }

        public VTigerSalesOrder(string subject, SoStatus soStatus, string billStreet,
            string shipStreet, Invoicestatus invoiceStatus, string accountId, string assignedUserId)
        {
            Subject = subject;
            SoStatus = soStatus;
            AccountId = accountId;
            AssignedUserId = assignedUserId;
            BillStreet = billStreet;
            ShipStreet = shipStreet;
            InvoiceStatus = invoiceStatus;
        }

        [JsonPropertyName("salesorder_no")]
        public string SalesOrderNo { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("potential_id")]
        public string PotentialId { get; set; }

        [JsonPropertyName("customerno")]
        public string CustomerNo { get; set; }

        [JsonPropertyName("quote_id")]
        public string QuoteId { get; set; }

        [JsonPropertyName("vtiger_purchaseorder")]
        public string VtigerPurchaseOrder { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("duedate")]
        public string DueDate { get; set; }

        [JsonPropertyName("carrier")]
        public string Carrier { get; set; }

        [JsonPropertyName("pending")]
        public string Pending { get; set; }

        [JsonPropertyName("sostatus")]
        public SoStatus SoStatus { get; set; } // mandatory

        [JsonPropertyName("txtAdjustment")]
        public double TxtAdjustment { get; set; }

        [JsonPropertyName("salescommission")]
        public double SalesCommission { get; set; }

        [JsonPropertyName("exciseduty")]
        public double ExciseDuty { get; set; }

        [JsonPropertyName("hdnGrandTotal")]
        public double HiddenGrandTotal { get; set; }

        [JsonPropertyName("hdnSubTotal")]
        public double HiddenSubTotal { get; set; }

        [JsonPropertyName("hdnTaxType")]
        public HdnTaxType HiddenTaxType { get; set; }

        [JsonPropertyName("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonPropertyName("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonPropertyName("hdnDiscountPercent")]
        public double HiddenDiscountPercent { get; set; }

        [JsonPropertyName("hdnDiscountAmount")]
        public double HiddenDiscountAmount { get; set; }

        [JsonPropertyName("hdnS_H_Amount")]
        public double HiddenShippingHandlingAmount { get; set; }

        [JsonPropertyName("hdnS_H_Percent")]
        public double HiddenShippingHandlingPercent { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("conversion_rate")]
        public double ConversionRate { get; set; }

        [JsonPropertyName("bill_street")]
        public string BillStreet { get; set; } // mandatory

        [JsonPropertyName("ship_street")]
        public string ShipStreet { get; set; } // mandatory

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("ship_city")]
        public string ShipCity { get; set; }

        [JsonPropertyName("bill_state")]
        public string BillState { get; set; }

        [JsonPropertyName("ship_state")]
        public string ShipState { get; set; }

        [JsonPropertyName("bill_code")]
        public string BillCode { get; set; }

        [JsonPropertyName("ship_code")]
        public string ShipCode { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("ship_country")]
        public string ShipCountry { get; set; }

        [JsonPropertyName("bill_pobox")]
        public string BillPoBox { get; set; }

        [JsonPropertyName("ship_pobox")]
        public string ShipPoBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("terms_conditions")]
        public string TermsConditions { get; set; }

        [JsonPropertyName("enable_recurring")]
        public bool EnableRecurring { get; set; }

        [JsonPropertyName("recurring_frequency")]
        public Recurring_frequency RecurringFrequency { get; set; }

        [JsonPropertyName("start_period")]
        public string StartPeriod { get; set; }

        [JsonPropertyName("end_period")]
        public string EndPeriod { get; set; }

        [JsonPropertyName("payment_duration")]
        public Payment_duration PaymentDuration { get; set; }

        [JsonPropertyName("invoicestatus")]
        public Invoicestatus InvoiceStatus { get; set; } // mandatory

        [JsonPropertyName("productid")]
        public string ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        [JsonPropertyName("listprice")]
        public double ListPrice { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tax1")]
        public string Tax1 { get; set; }

        [JsonPropertyName("tax2")]
        public string Tax2 { get; set; }

        [JsonPropertyName("tax3")]
        public string Tax3 { get; set; }

        [JsonPropertyName("pre_tax_total")]
        public double PreTaxTotal { get; set; }
    }

    /// <summary>
    /// VTiger-Invoice object
    /// </summary>
    public class VTigerInvoice : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerInvoice();
        }

        public override string RemoteTableName() { return "Invoice"; }

        public override VTigerType GetElementType() { return VTigerType.Invoice; }

        public VTigerInvoice() { }

        public VTigerInvoice(string subject, string billStreet, string shipStreet, string accountId, string assignedUserId)
        {
            Subject = subject;
            AccountId = accountId;
            AssignedUserId = assignedUserId;
            BillStreet = billStreet;
            ShipStreet = shipStreet;
        }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("salesorder_id")]
        public string SalesOrderId { get; set; }

        [JsonPropertyName("customerno")]
        public string CustomerNo { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }

        [JsonPropertyName("invoicedate")]
        public string InvoiceDate { get; set; }

        [JsonPropertyName("duedate")]
        public string DueDate { get; set; }

        [JsonPropertyName("vtiger_purchaseorder")]
        public string VtigerPurchaseOrder { get; set; }

        [JsonPropertyName("txtAdjustment")]
        public double TxtAdjustment { get; set; }

        [JsonPropertyName("salescommission")]
        public double SalesCommission { get; set; }

        [JsonPropertyName("exciseduty")]
        public double ExciseDuty { get; set; }

        [JsonPropertyName("hdnSubTotal")]
        public double HiddenSubTotal { get; set; }

        [JsonPropertyName("hdnGrandTotal")]
        public double HiddenGrandTotal { get; set; }

        [JsonPropertyName("hdnS_H_Amount")]
        public double HiddenShippingHandlingAmount { get; set; }

        [JsonPropertyName("hdnS_H_Percent")]
        public double HiddenShippingHandlingPercent { get; set; }

        [JsonPropertyName("hdnTaxType")]
        public HdnTaxType HiddenTaxType { get; set; }

        [JsonPropertyName("hdnDiscountPercent")]
        public double HiddenDiscountPercent { get; set; }

        [JsonPropertyName("hdnDiscountAmount")]
        public double HiddenDiscountAmount { get; set; }

        [JsonPropertyName("discount_percent")]
        public double DiscountPercent { get; set; }

        [JsonPropertyName("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } // mandatory

        [JsonPropertyName("invoicestatus")]
        public Invoicestatus InvoiceStatus { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("conversion_rate")]
        public double ConversionRate { get; set; }

        [JsonPropertyName("bill_street")]
        public string BillStreet { get; set; } // mandatory

        [JsonPropertyName("ship_street")]
        public string ShipStreet { get; set; } // mandatory

        [JsonPropertyName("bill_city")]
        public string BillCity { get; set; }

        [JsonPropertyName("ship_city")]
        public string ShipCity { get; set; }

        [JsonPropertyName("bill_state")]
        public string BillState { get; set; }

        [JsonPropertyName("ship_state")]
        public string ShipState { get; set; }

        [JsonPropertyName("bill_code")]
        public string BillCode { get; set; }

        [JsonPropertyName("ship_code")]
        public string ShipCode { get; set; }

        [JsonPropertyName("bill_country")]
        public string BillCountry { get; set; }

        [JsonPropertyName("ship_country")]
        public string ShipCountry { get; set; }

        [JsonPropertyName("bill_pobox")]
        public string BillPoBox { get; set; }

        [JsonPropertyName("ship_pobox")]
        public string ShipPoBox { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("terms_conditions")]
        public string TermsConditions { get; set; }

        [JsonPropertyName("invoice_no")]
        public string InvoiceNo { get; set; }

        [JsonPropertyName("productid")]
        public string ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public double Quantity { get; set; }

        [JsonPropertyName("listprice")]
        public double ListPrice { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tax1")]
        public string Tax1 { get; set; }

        [JsonPropertyName("tax2")]
        public string Tax2 { get; set; }

        [JsonPropertyName("tax3")]
        public string Tax3 { get; set; }

        [JsonPropertyName("pre_tax_total")]
        public double PreTaxTotal { get; set; }

        [JsonPropertyName("received")]
        public double Received { get; set; }

        [JsonPropertyName("balance")]
        public double Balance { get; set; }
    }

    /// <summary>
    /// VTiger-Campaigns object
    /// </summary>
    public class VTigerCampaign : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerCampaign();
        }

        public override string RemoteTableName() { return "Campaigns"; }

        public override VTigerType GetElementType() { return VTigerType.Campaigns; }

        public VTigerCampaign() { }

        public VTigerCampaign(string campaignName, DateTime closingDate, string assignedUserId)
        {
            CampaignName = campaignName;
            ClosingDate = VTiger.DateTimeToVtDate(closingDate);
            AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("campaignname")]
        public string CampaignName { get; set; } // mandatory

        [JsonPropertyName("campaign_no")]
        public string CampaignNo { get; set; }

        [JsonPropertyName("campaigntype")]
        public Campaigntype CampaignType { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("campaignstatus")]
        public Campaignstatus CampaignStatus { get; set; }

        [JsonPropertyName("closingdate")]
        public string ClosingDate { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("numsent")]
        public double NumSent { get; set; }

        [JsonPropertyName("sponsor")]
        public string Sponsor { get; set; }

        [JsonPropertyName("targetaudience")]
        public string TargetAudience { get; set; }

        [JsonPropertyName("targetsize")]
        public int TargetSize { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("expectedresponse")]
        public Expectedresponse ExpectedResponse { get; set; }

        [JsonPropertyName("expectedrevenue")]
        public double ExpectedRevenue { get; set; }

        [JsonPropertyName("budgetcost")]
        public double BudgetCost { get; set; }

        [JsonPropertyName("actualcost")]
        public double ActualCost { get; set; }

        [JsonPropertyName("expectedresponsecount")]
        public int ExpectedResponseCount { get; set; }

        [JsonPropertyName("expectedsalescount")]
        public int ExpectedSalesCount { get; set; }

        [JsonPropertyName("expectedroi")]
        public double ExpectedROI { get; set; }

        [JsonPropertyName("actualresponsecount")]
        public int ActualResponseCount { get; set; }

        [JsonPropertyName("actualsalescount")]
        public int ActualSalesCount { get; set; }

        [JsonPropertyName("actualroi")]
        public double ActualROI { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Events object
    /// </summary>
    public class VTigerEvent : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerEvent();
        }

        public override string RemoteTableName() { return "Events"; }

        public override VTigerType GetElementType() { return VTigerType.Events; }

        public VTigerEvent() { }

        public VTigerEvent(string subject, string dateStart, string timeStart, string dueDate,
            string timeEnd, int durationHours, Eventstatus eventStatus,
            Activitytype activityType, string assignedUserId)
        {
            Subject = subject;
            AssignedUserId = assignedUserId;
            DateStart = dateStart;
            TimeStart = timeStart;
            DueDate = dueDate;
            TimeEnd = timeEnd;
            DurationHours = durationHours;
            EventStatus = eventStatus;
            ActivityType = activityType;
        }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("date_start")]
        public string DateStart { get; set; } // mandatory

        [JsonPropertyName("time_start")]
        public string TimeStart { get; set; } // mandatory

        [JsonPropertyName("due_date")]
        public string DueDate { get; set; } // mandatory

        [JsonPropertyName("time_end")]
        public string TimeEnd { get; set; } // mandatory

        [JsonPropertyName("recurringtype")]
        public RecurringType RecurringType { get; set; }

        [JsonPropertyName("duration_hours")]
        public int DurationHours { get; set; } // mandatory

        [JsonPropertyName("duration_minutes")]
        public Duration_minutes DurationMinutes { get; set; }

        [JsonPropertyName("parent_id")]
        public string ParentId { get; set; }

        [JsonPropertyName("eventstatus")]
        public Eventstatus EventStatus { get; set; } // mandatory

        [JsonPropertyName("sendnotification")]
        public bool SendNotification { get; set; }

        [JsonPropertyName("activitytype")]
        public Activitytype ActivityType { get; set; } // mandatory

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("taskpriority")]
        public Taskpriority TaskPriority { get; set; }

        [JsonPropertyName("notime")]
        public bool NoTime { get; set; }

        [JsonPropertyName("visibility")]
        public Visibility Visibility { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("reminder_time")]
        public int ReminderTime { get; set; }

        [JsonPropertyName("contact_id")]
        public string ContactId { get; set; }
    }

    /// <summary>
    /// VTiger-Users object
    /// </summary>
    public class VTigerUser : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerUser();
        }

        public override string RemoteTableName() { return "User"; }

        public override VTigerType GetElementType() { return VTigerType.Users; }

        public VTigerUser() { }

        public VTigerUser(string userName, string userPassword, string confirmPassword, string lastName, string roleId, string email1)
        {
            UserName = userName;
            UserPassword = userPassword;
            ConfirmPassword = confirmPassword;
            LastName = lastName;
            RoleId = roleId;
            Email1 = email1;
        }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; } // mandatory

        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }

        [JsonPropertyName("user_password")]
        public string UserPassword { get; set; } // mandatory

        [JsonPropertyName("confirm_password")]
        public string ConfirmPassword { get; set; } // mandatory

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; } // mandatory

        [JsonPropertyName("roleid")]
        public string RoleId { get; set; } // mandatory

        [JsonPropertyName("email1")]
        public string Email1 { get; set; } // mandatory

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("activity_view")]
        public Activity_view ActivityView { get; set; }

        [JsonPropertyName("lead_view")]
        public Lead_view LeadView { get; set; }

        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("hour_format")]
        public string HourFormat { get; set; }

        [JsonPropertyName("end_hour")]
        public string EndHour { get; set; }

        [JsonPropertyName("start_hour")]
        public string StartHour { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("phone_work")]
        public string PhoneWork { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("phone_mobile")]
        public string PhoneMobile { get; set; }

        [JsonPropertyName("reports_to_id")]
        public string ReportsToId { get; set; }

        [JsonPropertyName("phone_other")]
        public string PhoneOther { get; set; }

        [JsonPropertyName("email2")]
        public string Email2 { get; set; }

        [JsonPropertyName("phone_fax")]
        public string PhoneFax { get; set; }

        [JsonPropertyName("yahoo_id")]
        public string YahooId { get; set; }

        [JsonPropertyName("phone_home")]
        public string PhoneHome { get; set; }

        [JsonPropertyName("date_format")]
        public Date_format DateFormat { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("address_street")]
        public string AddressStreet { get; set; }

        [JsonPropertyName("address_city")]
        public string AddressCity { get; set; }

        [JsonPropertyName("address_state")]
        public string AddressState { get; set; }

        [JsonPropertyName("address_postalcode")]
        public string AddressPostalCode { get; set; }

        [JsonPropertyName("address_country")]
        public string AddressCountry { get; set; }

        [JsonPropertyName("accesskey")]
        public string AccessKey { get; set; }

        [JsonPropertyName("internal_mailer")]
        public bool InternalMailer { get; set; }

        [JsonPropertyName("reminder_interval")]
        public Reminder_interval ReminderInterval { get; set; }

        [JsonPropertyName("asterisk_extension")]
        public string AsteriskExtension { get; set; }

        [JsonPropertyName("use_asterisk")]
        public bool UseAsterisk { get; set; }
    }

    /// <summary>
    /// VTiger-PBXManager object
    /// </summary>
    public class VTigerPBXManager : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerPBXManager();
        }

        public override string RemoteTableName() { return "PBXManager"; }

        public override VTigerType GetElementType() { return VTigerType.PBXManager; }

        public VTigerPBXManager() { }

        public VTigerPBXManager(string customerNumber, string callFrom, string callTo, string assignedUserId)
        {
            CallFrom = callFrom;
            CallTo = callTo;
            CustomerNumber = customerNumber;
            AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("callfrom")]
        public string CallFrom { get; set; } // mandatory

        [JsonPropertyName("callto")]
        public string CallTo { get; set; } // mandatory

        [JsonPropertyName("customernumber")]
        public string CustomerNumber { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("timeofcall")]
        public string TimeOfCall { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    /// <summary>
    /// VTiger-ServiceContracts object
    /// </summary>
    public class VTigerServiceContract : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerServiceContract();
        }

        public override string RemoteTableName() { return "ServiceContracts"; }

        public override VTigerType GetElementType() { return VTigerType.ServiceContracts; }

        public VTigerServiceContract() { }

        public VTigerServiceContract(string subject, string assignedUserId)
        {
            AssignedUserId = assignedUserId;
            Subject = subject;
        }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public string CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public string ModifiedTime { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public string EndDate { get; set; }

        [JsonPropertyName("sc_related_to")]
        public string RelatedTo { get; set; }

        [JsonPropertyName("tracking_unit")]
        public Tracking_unit TrackingUnit { get; set; }

        [JsonPropertyName("total_units")]
        public string TotalUnits { get; set; }

        [JsonPropertyName("used_units")]
        public string UsedUnits { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } // mandatory

        [JsonPropertyName("due_date")]
        public string DueDate { get; set; }

        [JsonPropertyName("planned_duration")]
        public string PlannedDuration { get; set; }

        [JsonPropertyName("actual_duration")]
        public string ActualDuration { get; set; }

        [JsonPropertyName("contract_status")]
        public Contract_status ContractStatus { get; set; }

        [JsonPropertyName("contract_priority")]
        public Contract_priority ContractPriority { get; set; }

        [JsonPropertyName("contract_type")]
        public Contract_type ContractType { get; set; }

        [JsonPropertyName("progress")]
        public double Progress { get; set; }

        [JsonPropertyName("contract_no")]
        public string ContractNo { get; set; }
    }

    /// <summary>
    /// VTiger-Services object
    /// </summary>
    public class VTigerService : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerService();
        }

        public override string RemoteTableName() { return "Services"; }

        public override VTigerType GetElementType() { return VTigerType.Services; }

        public VTigerService() { }

        public VTigerService(string serviceName)
        {
            ServiceName = serviceName;
        }

        [JsonPropertyName("servicename")]
        public string ServiceName { get; set; } // mandatory

        [JsonPropertyName("service_no")]
        public string ServiceNo { get; set; }

        [JsonPropertyName("discontinued")]
        public bool Discontinued { get; set; }

        [JsonPropertyName("sales_start_date")]
        public string SalesStartDate { get; set; }

        [JsonPropertyName("sales_end_date")]
        public string SalesEndDate { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("service_usageunit")]
        public Service_usageunit ServiceUsageUnit { get; set; }

        [JsonPropertyName("qty_per_unit")]
        public double QuantityPerUnit { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; }

        [JsonPropertyName("servicecategory")]
        public Servicecategory ServiceCategory { get; set; }

        [JsonPropertyName("unit_price")]
        public double UnitPrice { get; set; }

        [JsonPropertyName("taxclass")]
        public string TaxClass { get; set; }

        [JsonPropertyName("commissionrate")]
        public double CommissionRate { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Assets object
    /// </summary>
    public class VTigerAsset : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerAsset();
        }

        public override string RemoteTableName() { return "Assets"; }

        public override VTigerType GetElementType() { return VTigerType.Assets; }

        public VTigerAsset() { }

        public VTigerAsset(string product, string serialNumber, string dateSold,
            string dateInService, Assetstatus assetStatus, string assetName,
            string account, string assignedUserId)
        {
            Product = product;
            SerialNumber = serialNumber;
            DateSold = dateSold;
            DateInService = dateInService;
            AssetStatus = assetStatus;
            AssignedUserId = assignedUserId;
            AssetName = assetName;
            Account = account;
        }

        [JsonPropertyName("asset_no")]
        public string AssetNo { get; set; }

        [JsonPropertyName("product")]
        public string Product { get; set; } // mandatory

        [JsonPropertyName("serialnumber")]
        public string SerialNumber { get; set; } // mandatory

        [JsonPropertyName("datesold")]
        public string DateSold { get; set; } // mandatory

        [JsonPropertyName("dateinservice")]
        public string DateInService { get; set; } // mandatory

        [JsonPropertyName("assetstatus")]
        public Assetstatus AssetStatus { get; set; } // mandatory

        [JsonPropertyName("tagnumber")]
        public string TagNumber { get; set; }

        [JsonPropertyName("invoiceid")]
        public string InvoiceId { get; set; }

        [JsonPropertyName("shippingmethod")]
        public string ShippingMethod { get; set; }

        [JsonPropertyName("shippingtrackingnumber")]
        public string ShippingTrackingNumber { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("assetname")]
        public string AssetName { get; set; } // mandatory

        [JsonPropertyName("account")]
        public string Account { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-ModComments object
    /// </summary>
    public class VTigerModComment : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerModComment();
        }

        public override string RemoteTableName() { return "ModComments"; }

        public override VTigerType GetElementType() { return VTigerType.ModComments; }

        public VTigerModComment() { }

        public VTigerModComment(string commentContent, string assignedUserId, string relatedTo)
        {
            CommentContent = commentContent;
            AssignedUserId = assignedUserId;
            RelatedTo = relatedTo;
        }

        [JsonPropertyName("commentcontent")]
        public string CommentContent { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("related_to")]
        public string RelatedTo { get; set; } // mandatory

        [JsonPropertyName("creator")]
        public string Creator { get; set; }

        [JsonPropertyName("parent_comments")]
        public string ParentComments { get; set; }
    }

    /// <summary>
    /// VTiger-ProjectMilestone object
    /// </summary>
    public class VTigerProjectMilestone : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerProjectMilestone();
        }

        public override string RemoteTableName() { return "ProjectMilestone"; }

        public override VTigerType GetElementType() { return VTigerType.ProjectMilestone; }

        public VTigerProjectMilestone() { }

        public VTigerProjectMilestone(string projectMilestoneName, string projectId, string assignedUserId)
        {
            ProjectMilestoneName = projectMilestoneName;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("projectmilestonename")]
        public string ProjectMilestoneName { get; set; } // mandatory

        [JsonPropertyName("projectmilestonedate")]
        public string ProjectMilestoneDate { get; set; }

        [JsonPropertyName("projectid")]
        public string ProjectId { get; set; } // mandatory

        [JsonPropertyName("projectmilestonetype")]
        public Projectmilestonetype ProjectMilestoneType { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("projectmilestone_no")]
        public string ProjectMilestoneNo { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-ProjectTask object
    /// </summary>
    public class VTigerProjectTask : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerProjectTask();
        }

        public override string RemoteTableName() { return "ProjectTask"; }

        public override VTigerType GetElementType() { return VTigerType.ProjectTask; }

        public VTigerProjectTask() { }

        public VTigerProjectTask(string projectTaskName, string projectId, string assignedUserId)
        {
            ProjectTaskName = projectTaskName;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("projecttaskname")]
        public string ProjectTaskName { get; set; } // mandatory

        [JsonPropertyName("projecttasktype")]
        public Projecttasktype ProjectTaskType { get; set; }

        [JsonPropertyName("projecttaskpriority")]
        public Projecttaskpriority ProjectTaskPriority { get; set; }

        [JsonPropertyName("projectid")]
        public string ProjectId { get; set; } // mandatory

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("projecttasknumber")]
        public int ProjectTaskNumber { get; set; }

        [JsonPropertyName("projecttask_no")]
        public string ProjectTaskNo { get; set; }

        [JsonPropertyName("projecttaskprogress")]
        public Projecttaskprogress ProjectTaskProgress { get; set; }

        [JsonPropertyName("projecttaskhours")]
        public string ProjectTaskHours { get; set; }

        [JsonPropertyName("startdate")]
        public string StartDate { get; set; }

        [JsonPropertyName("enddate")]
        public string EndDate { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Project object
    /// </summary>
    public class VTigerProject : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerProject();
        }

        public override string RemoteTableName() { return "Project"; }

        public override VTigerType GetElementType() { return VTigerType.Project; }

        public VTigerProject() { }

        public VTigerProject(string projectName, string assignedUserId)
        {
            ProjectName = projectName;
            AssignedUserId = assignedUserId;
        }

        [JsonPropertyName("projectname")]
        public string ProjectName { get; set; } // mandatory

        [JsonPropertyName("startdate")]
        public string StartDate { get; set; }

        [JsonPropertyName("targetenddate")]
        public string TargetEndDate { get; set; }

        [JsonPropertyName("actualenddate")]
        public string ActualEndDate { get; set; }

        [JsonPropertyName("projectstatus")]
        public Projectstatus ProjectStatus { get; set; }

        [JsonPropertyName("projecttype")]
        public Projecttype ProjectType { get; set; }

        [JsonPropertyName("linktoaccountscontacts")]
        public string LinkToAccountsContacts { get; set; }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("project_no")]
        public string ProjectNo { get; set; }

        [JsonPropertyName("targetbudget")]
        public string TargetBudget { get; set; }

        [JsonPropertyName("projecturl")]
        public string ProjectUrl { get; set; }

        [JsonPropertyName("projectpriority")]
        public Projectpriority ProjectPriority { get; set; }

        [JsonPropertyName("progress")]
        public Progress Progress { get; set; }

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-SMSNotifier object
    /// </summary>
    public class VTigerSMSNotifier : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerSMSNotifier();
        }

        public override string RemoteTableName() { return "SMSNotifier"; }

        public override VTigerType GetElementType() { return VTigerType.SMSNotifier; }

        public VTigerSMSNotifier() { }

        public VTigerSMSNotifier(string assignedUserId, string message)
        {
            AssignedUserId = assignedUserId;
            Message = message;
        }

        [JsonPropertyName("assigned_user_id")]
        public string AssignedUserId { get; set; } // mandatory

        [JsonPropertyName("createdtime")]
        public DateTime CreatedTime { get; set; }

        [JsonPropertyName("modifiedtime")]
        public DateTime ModifiedTime { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } // mandatory
    }

    /// <summary>
    /// VTiger-Groups object
    /// </summary>
    public class VTigerGroup : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerGroup();
        }

        public override string RemoteTableName() { return "Groups"; }

        public override VTigerType GetElementType() { return VTigerType.Groups; }

        [JsonPropertyName("groupname")]
        public string GroupName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// VTiger-Currency object
    /// </summary>
    public class VTigerCurrency : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerCurrency();
        }

        public override string RemoteTableName() { return "Currency"; }

        public override VTigerType GetElementType() { return VTigerType.Currency; }

        public VTigerCurrency() { }

        public VTigerCurrency(string defaultId, int deleted)
        {
            DefaultId = defaultId;
            Deleted = deleted;
        }

        [JsonPropertyName("currency_name")]
        public string CurrencyName { get; set; }

        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonPropertyName("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonPropertyName("conversion_rate")]
        public double ConversionRate { get; set; }

        [JsonPropertyName("currency_status")]
        public string CurrencyStatus { get; set; }

        [JsonPropertyName("defaultid")]
        public string DefaultId { get; set; } // mandatory

        [JsonPropertyName("deleted")]
        public int Deleted { get; set; } // mandatory
    }

    /// <summary>
    /// VTiger-DocumentFolders object
    /// </summary>
    public class VTigerDocumentFolder : VTigerEntity
    {
        public override VTigerEntity CreateNewInstance()
        {
            return new VTigerDocumentFolder();
        }

        public override string RemoteTableName() { return "DocumentFolders"; }

        public override VTigerType GetElementType() { return VTigerType.DocumentFolders; }

        public VTigerDocumentFolder() { }

        public VTigerDocumentFolder(string folderName, string createdBy)
        {
            FolderName = folderName;
            CreatedBy = createdBy;
        }

        [JsonPropertyName("foldername")]
        public string FolderName { get; set; } // mandatory

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("createdby")]
        public string CreatedBy { get; set; } // mandatory

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }
    }

    #endregion

}