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
using Jayrock.Json;
using Jayrock.Json.Conversion;

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

    public class VTigerApiException : System.Exception
    {
        internal VTigerApiException(VTigerError apiError)
        {
            this.VTigerErrorCode = apiError.code;
            this.VTigerMessage = apiError.message;
            /*
            if ((apiError.code == null) || (apiError.code == ""))
            {
                this.Message = "UNKNOWN ERROR: " + apiError.message;
            }
            else
            {
                this.Message = apiError.code + ": " + apiError.message;
            }
            */
        }
        public string VTigerErrorCode;
        public string VTigerMessage;
        public override string Message
        {
            get
            {
                if ((this.VTigerErrorCode == null) || (this.VTigerErrorCode == ""))
                {
                    return "UNKNOWN ERROR: " + this.VTigerMessage;
                }
                else
                {
                    return this.VTigerErrorCode + ": " + this.VTigerMessage;
                }
            }
        }

    }
    //====================================================================
    #region VTiger-Access-Classes

    public class VTigerResult<T>
    {
        public bool success;
        public VTigerError error;
        public T result; 
    }

    public class VTigerError
    {
        public string code;
        public string message;
    }

    public class VTigerLogin
    {
        public string sessionName;
        public string userId;
        public string version;
        public string vtigerVersion;
    }

    public class VTigerToken
    {
        public string token;
        public int serverTime;
        public int expireTime;
    }

    public class VTigerTypes
    {
        public string[] types;
        public VTigerTypeInfo[] typeInfo;
        public JsonObject information;
    }

    public class VTigerTypeInfo
    {
        public string Name;
        public string label;
        public string singular;
        public bool isEntity;
    }

    //====================================================================

    /// <summary>
    /// Containts the description of a VTiger-object
    /// </summary>
    public class VTigerObjectType
    {
        public string label;
        public string name;
        public bool createable;
        public bool updateable;
        public bool deleteable;
        public bool retrieveable;
        public VTigerObjectField[] fields;
        public string idPrefix;
        public string isEntity;
        public string labelFields;
    }
    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerObjectField
    {
        public string label;
        public string name;
        public bool mandatory;
        public VTigerTypeDesc type;
        public bool nullable;
        public bool editable;
    }
    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerTypeDesc
    {
        public string name;
        public VTigerPicklistItem[] picklistValues;
        public string[] refersTo;
    }
    /// <summary>
    /// Part of VTigerObjectType
    /// </summary>
    public class VTigerPicklistItem
    {
        public string label;
        public string value;
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
        Undefined,
        Calendar, Leads, Accounts, Contacts, Potentials, Products,
        Documents, Emails, HelpDesk, Faq, Vendors, PriceBooks, Quotes,
        PurchaseOrder, SalesOrder, Invoice, Campaigns, Events, Users,
        PBXManager, ServiceContracts, Services, Assets, ModComments,
        ProjectMilestone, ProjectTask, Project, SMSNotifier, Groups,
        Currency, DocumentFolders
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
        public VTigerType elementType { get { return GetElementType(); } }
        public virtual VTigerType GetElementType()
        {
            return VTigerType.Undefined;
        }
        public string id;
    }

    /// <summary>
    /// VTiger-Calendar object
    /// </summary>
    public class VTigerCalendar : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Calendar; }
        public VTigerCalendar() { }
        public VTigerCalendar(string subject, string assigned_user_id, string date_start, string due_date, TaskStatus taskstatus)
        {
            this.subject = subject;
            this.assigned_user_id = assigned_user_id;
            this.date_start = date_start;
            this.due_date = due_date;
            this.taskstatus = taskstatus;
        }
        public string subject; //mandatory
        public string assigned_user_id; //mandatory
        public string date_start; //mandatory
        public string time_start;
        public string time_end;
        public string due_date; //mandatory
        public string parent_id;
        public string contact_id;
        public TaskStatus taskstatus; //mandatory
        public Eventstatus eventstatus;
        public Taskpriority taskpriority;
        public bool sendnotification;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public Activitytype activitytype;
        public Visibility visibility;
        public string description;
        public string duration_hours;
        public Duration_minutes duration_minutes;
        public string location;
        public int reminder_time;
        public RecurringType recurringtype;
        public bool notime;
    }

    /// <summary>
    /// VTiger-Leads object
    /// </summary>
    public class VTigerLead : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Leads; }
        public VTigerLead() { }
        public VTigerLead(string lastname, string company, string assigned_user_id)
        {
            this.lastname = lastname;
            this.company = company;
            this.assigned_user_id = assigned_user_id;
        }
        public string salutationtype;
        public string firstname;
        public string lead_no;
        public string phone;
        public string lastname; //mandatory
        public string mobile;
        public string company; //mandatory
        public string fax;
        public string designation;
        public string email;
        public Leadsource leadsource;
        public string website;
        public Industry industry;
        public Leadstatus leadstatus;
        public int annualrevenue;
        public Rating rating;
        public int noofemployees;
        public string assigned_user_id; //mandatory
        public string yahooid;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string lane;
        public string code;
        public string city;
        public string country;
        public string state;
        public string pobox;
        public string description;
    }

    /// <summary>
    /// VTiger-Accounts object
    /// </summary>
    public class VTigerAccount : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Accounts; }
        public VTigerAccount() { }
        public VTigerAccount(string accountname, string assigned_user_id)
        {
            this.accountname = accountname;
            this.assigned_user_id = assigned_user_id;
        }
        public string accountname; //mandatory
        public string account_no;
        public string phone;
        public string website;
        public string fax;
        public string tickersymbol;
        public string otherphone;
        public string account_id;
        public string email1;
        public int employees;
        public string email2;
        public string ownership;
        public Rating rating;
        public Industry industry;
        public string siccode;
        public Accounttype accounttype;
        public int annual_revenue;
        public bool emailoptout;
        public bool notify_owner;
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string bill_street;
        public string ship_street;
        public string bill_city;
        public string ship_city;
        public string bill_state;
        public string ship_state;
        public string bill_code;
        public string ship_code;
        public string bill_country;
        public string ship_country;
        public string bill_pobox;
        public string ship_pobox;
        public string description;
    }

    /// <summary>
    /// VTiger-Contacts object
    /// </summary>
    public class VTigerContact : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Contacts; }
        public VTigerContact() { }
        public VTigerContact(string lastname, string assigned_user_id)
        {
            this.lastname = lastname;
            this.assigned_user_id = assigned_user_id;
        }
        public string salutationtype;
        public string firstname;
        public string contact_no;
        public string phone;
        public string lastname; //mandatory
        public string mobile;
        public string account_id;
        public string homephone;
        public Leadsource leadsource;
        public string otherphone;
        public string title;
        public string fax;
        public string department;
        public string birthday;
        public string email;
        public string contact_id;
        public string assistant;
        public string yahooid;
        public string assistantphone;
        public bool donotcall;
        public bool emailoptout;
        public string assigned_user_id; //mandatory
        public bool reference;
        public bool notify_owner;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public bool portal;
        public string support_start_date;
        public string support_end_date;
        public string mailingstreet;
        public string otherstreet;
        public string mailingcity;
        public string othercity;
        public string mailingstate;
        public string otherstate;
        public string mailingzip;
        public string otherzip;
        public string mailingcountry;
        public string othercountry;
        public string mailingpobox;
        public string otherpobox;
        public string description;
    }

    /// <summary>
    /// VTiger-Potentials object
    /// </summary>
    public class VTigerPotential : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Potentials; }
        public VTigerPotential() { }
        public VTigerPotential(string potentialname, string related_to, string closingdate, Sales_stage sales_stage, string assigned_user_id)
        {
            this.potentialname = potentialname;
            this.related_to = related_to;
            this.closingdate = closingdate;
            this.sales_stage = sales_stage;
            this.assigned_user_id = assigned_user_id;
        }
        public string potentialname; //mandatory
        public string potential_no;
        public double amount;
        public string related_to; //mandatory
        public string closingdate; //mandatory
        public Opportunity_type opportunity_type;
        public string nextstep;
        public Leadsource leadsource;
        public Sales_stage sales_stage; //mandatory
        public string assigned_user_id; //mandatory
        public double probability;
        public string campaignid;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string description;
    }

    /// <summary>
    /// VTiger-Products object
    /// </summary>
    public class VTigerProduct : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Products; }
        public VTigerProduct() { }
        public VTigerProduct(string productname)
        {
            this.productname = productname;
        }
        public string productname; //mandatory
        public string product_no;
        public string productcode;
        public bool discontinued;
        public string manufacturer;
        public string productcategory;
        public string sales_start_date;
        public string sales_end_date;
        public string start_date;
        public string expiry_date;
        public string website;
        public string vendor_id;
        public string mfr_part_no;
        public string vendor_part_no;
        public string serial_no;
        public string productsheet;
        public string glacct;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public double unit_price;
        public double commissionrate;
        public string taxclass;
        public string usageunit;
        public double qty_per_unit;
        public double qtyinstock;
        public int reorderlevel;
        public string assigned_user_id;
        public int qtyindemand;
        public string description;
    }

    /// <summary>
    /// VTiger-Documents object
    /// </summary>
    public class VTigerDocument : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Documents; }
        public VTigerDocument() { }
        public VTigerDocument(string notes_title, string assigned_user_id)
        {
            this.notes_title = notes_title;
            this.assigned_user_id = assigned_user_id;
        }
        public string notes_title; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string filename;
        public string assigned_user_id; //mandatory
        public string notecontent;
        public string filetype;
        public int filesize;
        public string filelocationtype;
        public string fileversion;
        public bool filestatus;
        public int filedownloadcount;
        public string folderid;
        public string note_no;
    }

    /// <summary>
    /// VTiger-Emails object
    /// </summary>
    public class VTigerEmail : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Emails; }
        public VTigerEmail() { }
        public VTigerEmail(string subject, DateTime date_start, string from_email, string[] saved_toid, string assigned_user_id)
        {
            this.date_start = VTiger.DateTimeToVtDate(date_start);
            this.assigned_user_id = assigned_user_id;
            this.subject = subject;
            this.from_email = from_email;
            //this.saved_toid = JsonConvert.ExportToString(saved_toid);
            this.saved_toid.Adresses = saved_toid;
            this.activitytype = "Emails";
        }
        public string date_start; //mandatory
        [JsonExcludeExportAttribute]
        public string parent_type; //Read-only
        public string activitytype;
        public string assigned_user_id; //mandatory
        public string subject; //mandatory
        public string filename;
        public string description;
        public string time_start;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string access_count;
        public string from_email; //mandatory
        public EmailAdresses saved_toid; //mandatory
        public EmailAdresses ccmail;
        public EmailAdresses bccmail;
        public string parent_id;
        public Email_flag email_flag;
    }

    /// <summary>
    /// VTiger-HelpDesk object
    /// </summary>
    public class VTigerHelpDesk : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.HelpDesk; }
        public VTigerHelpDesk() { }
        public VTigerHelpDesk(string assigned_user_id, Ticketstatus ticketstatus, string ticket_title)
        {
            this.assigned_user_id = assigned_user_id;
            this.ticketstatus = ticketstatus;
            this.ticket_title = ticket_title;
        }
        public string ticket_no;
        public string assigned_user_id; //mandatory
        public string parent_id;
        public Ticketpriorities ticketpriorities;
        public string product_id;
        public Ticketseverities ticketseverities;
        public Ticketstatus ticketstatus; //mandatory
        public Ticketcategories ticketcategories;
        public string update_log;
        public int hours;
        public int days;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string ticket_title; //mandatory
        public string description;
        public string solution;
    }

    /// <summary>
    /// VTiger-Faq object
    /// </summary>
    public class VTigerFaq : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Faq; }
        public VTigerFaq() { }
        public VTigerFaq(Faqstatus faqstatus, string question, string faq_answer)
        {
            this.faqstatus = faqstatus;
            this.question = question;
            this.faq_answer = faq_answer;
        }
        public string product_id;
        public string faq_no;
        public Faqcategories faqcategories;
        public Faqstatus faqstatus; //mandatory
        public string question; //mandatory
        public string faq_answer; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
    }

    /// <summary>
    /// VTiger-Vendors object
    /// </summary>
    public class VTigerVendor : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Vendors; }
        public VTigerVendor() { }
        public VTigerVendor(string vendorname)
        {
            this.vendorname = vendorname;
        }
        public string vendorname; //mandatory
        public string vendor_no;
        public string phone;
        public string email;
        public string website;
        public string glacct;
        public string category;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string street;
        public string pobox;
        public string city;
        public string state;
        public string postalcode;
        public string country;
        public string description;
    }

    /// <summary>
    /// VTiger-PriceBooks object
    /// </summary>
    public class VTigerPriceBook : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.PriceBooks; }
        public VTigerPriceBook() { }
        public VTigerPriceBook(string bookname, string currency_id)
        {
            this.bookname = bookname;
            this.currency_id = currency_id;
        }
        public string bookname; //mandatory
        public string pricebook_no;
        public bool active;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string currency_id; //mandatory
        public string description;
    }

    /// <summary>
    /// VTiger-Quotes object
    /// </summary>
    public class VTigerQuote : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Quotes; }
        public VTigerQuote() { }
        public VTigerQuote(string subject, Quotestage quotestage, string bill_street, 
            string ship_street, string account_id, string assigned_user_id)
        {
            this.subject = subject;
            this.quotestage = quotestage;
            this.account_id = account_id;
            this.assigned_user_id = assigned_user_id;
            this.bill_street = bill_street;
            this.ship_street = ship_street;
        }
        public string quote_no;
        public string subject; //mandatory
        public string potential_id;
        public Quotestage quotestage; //mandatory
        public string validtill;
        public string contact_id;
        public string carrier;
        public double hdnSubTotal;
        public string shipping;
        public string assigned_user_id1;
        public double txtAdjustment;
        public double hdnGrandTotal;
        public HdnTaxType hdnTaxType;
        public double hdnDiscountPercent;
        public double hdnDiscountAmount;
        public double hdnS_H_Amount;
        public string account_id; //mandatory
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string currency_id;
        public double conversion_rate;
        public string bill_street; //mandatory
        public string ship_street; //mandatory
        public string bill_city;
        public string ship_city;
        public string bill_state;
        public string ship_state;
        public string bill_code;
        public string ship_code;
        public string bill_country;
        public string ship_country;
        public string bill_pobox;
        public string ship_pobox;
        public string description;
        public string terms_conditions;
    }

    /// <summary>
    /// VTiger-PurchaseOrder object
    /// </summary>
    public class VTigerPurchaseOrder : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.PurchaseOrder; }
        public VTigerPurchaseOrder() { }
        public VTigerPurchaseOrder(string subject, string vendor_id, PoStatus postatus,
            string bill_street, string ship_street, string assigned_user_id)
        {
            this.subject = subject;
            this.vendor_id = vendor_id;
            this.postatus = postatus;
            this.assigned_user_id = assigned_user_id;
            this.bill_street = bill_street;
            this.ship_street = ship_street;
        }
        public string purchaseorder_no;
        public string subject; //mandatory
        public string vendor_id; //mandatory
        public string requisition_no;
        public string tracking_no;
        public string contact_id;
        public string duedate;
        public string carrier;
        public double txtAdjustment;
        public double salescommission;
        public double exciseduty;
        public double hdnGrandTotal;
        public double hdnSubTotal;
        public HdnTaxType hdnTaxType;
        public double hdnDiscountPercent;
        public double hdnDiscountAmount;
        public double hdnS_H_Amount;
        public PoStatus postatus; //mandatory
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string currency_id;
        public double conversion_rate;
        public string bill_street; //mandatory
        public string ship_street; //mandatory
        public string bill_city;
        public string ship_city;
        public string bill_state;
        public string ship_state;
        public string bill_code;
        public string ship_code;
        public string bill_country;
        public string ship_country;
        public string bill_pobox;
        public string ship_pobox;
        public string description;
        public string terms_conditions;
    }

    /// <summary>
    /// VTiger-SalesOrder object
    /// </summary>
    public class VTigerSalesOrder : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.SalesOrder; }
        public VTigerSalesOrder() { }
        public VTigerSalesOrder(string subject, SoStatus sostatus, string bill_street, 
            string ship_street, Invoicestatus invoicestatus, string account_id, string assigned_user_id)
        {
            this.subject = subject;
            this.sostatus = sostatus;
            this.account_id = account_id;
            this.assigned_user_id = assigned_user_id;
            this.bill_street = bill_street;
            this.ship_street = ship_street;
            this.invoicestatus = invoicestatus;
        }
        public string salesorder_no;
        public string subject; //mandatory
        public string potential_id;
        public string customerno;
        public string quote_id;
        public string vtiger_purchaseorder;
        public string contact_id;
        public string duedate;
        public string carrier;
        public string pending;
        public SoStatus sostatus; //mandatory
        public double txtAdjustment;
        public double salescommission;
        public double exciseduty;
        public double hdnGrandTotal;
        public double hdnSubTotal;
        public HdnTaxType hdnTaxType;
        public double hdnDiscountPercent;
        public double hdnDiscountAmount;
        public double hdnS_H_Amount;
        public string account_id; //mandatory
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string currency_id;
        public double conversion_rate;
        public string bill_street; //mandatory
        public string ship_street; //mandatory
        public string bill_city;
        public string ship_city;
        public string bill_state;
        public string ship_state;
        public string bill_code;
        public string ship_code;
        public string bill_country;
        public string ship_country;
        public string bill_pobox;
        public string ship_pobox;
        public string description;
        public string terms_conditions;
        public bool enable_recurring;
        public Recurring_frequency recurring_frequency;
        public string start_period;
        public string end_period;
        public Payment_duration payment_duration;
        public Invoicestatus invoicestatus; //mandatory
    }

    /// <summary>
    /// VTiger-Invoice object
    /// </summary>
    public class VTigerInvoice : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Invoice; }
        public VTigerInvoice() { }
        public VTigerInvoice(string subject, string bill_street, string ship_street, 
            string account_id, string assigned_user_id)
        {
            this.subject = subject;
            this.account_id = account_id;
            this.assigned_user_id = assigned_user_id;
            this.bill_street = bill_street;
            this.ship_street = ship_street;
        }
        public string subject; //mandatory
        public string salesorder_id;
        public string customerno;
        public string contact_id;
        public string invoicedate;
        public string duedate;
        public string vtiger_purchaseorder;
        public double txtAdjustment;
        public double salescommission;
        public double exciseduty;
        public double hdnSubTotal;
        public double hdnGrandTotal;
        public HdnTaxType hdnTaxType;
        public double hdnDiscountPercent;
        public double hdnDiscountAmount;
        public string account_id; //mandatory
        public Invoicestatus invoicestatus;
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string currency_id;
        public double conversion_rate;
        public string bill_street; //mandatory
        public string ship_street; //mandatory
        public string bill_city;
        public string ship_city;
        public string bill_state;
        public string ship_state;
        public string bill_code;
        public string ship_code;
        public string bill_country;
        public string ship_country;
        public string bill_pobox;
        public string ship_pobox;
        public string description;
        public string terms_conditions;
        public string invoice_no;
    }

    /// <summary>
    /// VTiger-Campaigns object
    /// </summary>
    public class VTigerCampaign : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Campaigns; }
        public VTigerCampaign() { }
        public VTigerCampaign(string campaignname, DateTime closingDate, string assigned_user_id)
        {
            this.campaignname = campaignname;
            this.closingdate = VTiger.DateTimeToVtDate(closingDate);
            this.assigned_user_id = assigned_user_id;
        }
        public string campaignname; //mandatory
        public string campaign_no;
        public Campaigntype campaigntype;
        public string product_id;
        public Campaignstatus campaignstatus;
        public string closingdate; //mandatory
        public string assigned_user_id; //mandatory
        public double numsent;
        public string sponsor;
        public string targetaudience;
        public int targetsize;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public Expectedresponse expectedresponse;
        public double expectedrevenue;
        public double budgetcost;
        public double actualcost;
        public int expectedresponsecount;
        public int expectedsalescount;
        public double expectedroi;
        public int actualresponsecount;
        public int actualsalescount;
        public double actualroi;
        public string description;
    }

    /// <summary>
    /// VTiger-Events object
    /// </summary>
    public class VTigerEvent : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Events; }
        public VTigerEvent() { }
        public VTigerEvent(string subject, string date_start, string time_start, string due_date, 
            string time_end, int duration_hours, Eventstatus eventstatus, 
            Activitytype activitytype, string assigned_user_id)
        {
            this.subject = subject;
            this.assigned_user_id = assigned_user_id;
            this.date_start = date_start;
            this.time_start = time_start;
            this.due_date = due_date;
            this.time_end = time_end;
            this.duration_hours = duration_hours;
            this.eventstatus = eventstatus;
            this.activitytype = activitytype;
        }
        public string subject; //mandatory
        public string assigned_user_id; //mandatory
        public string date_start; //mandatory
        public string time_start; //mandatory
        public string due_date; //mandatory
        public string time_end; //mandatory
        public RecurringType recurringtype;
        public int duration_hours; //mandatory
        public Duration_minutes duration_minutes;
        public string parent_id;
        public Eventstatus eventstatus; //mandatory
        public bool sendnotification;
        public Activitytype activitytype; //mandatory
        public string location;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public Taskpriority taskpriority;
        public bool notime;
        public Visibility visibility;
        public string description;
        public int reminder_time;
        public string contact_id;
    }

    /// <summary>
    /// VTiger-Users object
    /// </summary>
    public class VTigerUser : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Users; }
        public VTigerUser() { }
        public VTigerUser(string user_name, string user_password, string confirm_password, string last_name, string roleid, string email1)
        {
            this.user_name = user_name;
            this.user_password = user_password;
            this.confirm_password = confirm_password;
            this.last_name = last_name;
            this.roleid = roleid;
            this.email1 = email1;
        }
        public string user_name; //mandatory
        public bool is_admin;
        public string user_password; //mandatory
        public string confirm_password; //mandatory
        public string first_name;
        public string last_name; //mandatory
        public string roleid; //mandatory
        public string email1; //mandatory
        public string status;
        public Activity_view activity_view;
        public Lead_view lead_view;
        public string currency_id;
        public string hour_format;
        public string end_hour;
        public string start_hour;
        public string title;
        public string phone_work;
        public string department;
        public string phone_mobile;
        public string reports_to_id;
        public string phone_other;
        public string email2;
        public string phone_fax;
        public string yahoo_id;
        public string phone_home;
        public Date_format date_format;
        public string signature;
        public string description;
        public string address_street;
        public string address_city;
        public string address_state;
        public string address_postalcode;
        public string address_country;
        public string accesskey;
        public bool internal_mailer;
        public Reminder_interval reminder_interval;
        public string asterisk_extension;
        public bool use_asterisk;
    }

    /// <summary>
    /// VTiger-PBXManager object
    /// </summary>
    public class VTigerPBXManager : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.PBXManager; }
        public VTigerPBXManager() { }
        public VTigerPBXManager(string callfrom, string callto)
        {
            this.callfrom = callfrom;
            this.callto = callto;
        }
        public string callfrom; //mandatory
        public string callto; //mandatory
        public string timeofcall;
        public string status;
    }

    /// <summary>
    /// VTiger-ServiceContracts object
    /// </summary>
    public class VTigerServiceContract : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.ServiceContracts; }
        public VTigerServiceContract() { }
        public VTigerServiceContract(string subject, string assigned_user_id)
        {
            this.assigned_user_id = assigned_user_id;
            this.subject = subject;
        }
        public string assigned_user_id; //mandatory
        public string createdtime;
        public string modifiedtime;
        public string start_date;
        public string end_date;
        public string sc_related_to;
        public Tracking_unit tracking_unit;
        public string total_units;
        public string used_units;
        public string subject; //mandatory
        public string due_date;
        public string planned_duration;
        public string actual_duration;
        public Contract_status contract_status;
        public Contract_priority contract_priority;
        public Contract_type contract_type;
        public double progress;
        public string contract_no;
    }

    /// <summary>
    /// VTiger-Services object
    /// </summary>
    public class VTigerService : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Services; }
        public VTigerService() { }
        public VTigerService(string servicename)
        {
            this.servicename = servicename;
        }
        public string servicename; //mandatory
        public string service_no;
        public bool discontinued;
        public string sales_start_date;
        public string sales_end_date;
        public string start_date;
        public string expiry_date;
        public string website;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public Service_usageunit service_usageunit;
        public double qty_per_unit;
        public string assigned_user_id;
        public Servicecategory servicecategory;
        public double unit_price;
        public string taxclass;
        public double commissionrate;
        public string description;
    }

    /// <summary>
    /// VTiger-Assets object
    /// </summary>
    public class VTigerAsset : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Assets; }
        public VTigerAsset() { }
        public VTigerAsset(string product, string serialnumber, string datesold, 
            string dateinservice, Assetstatus assetstatus, string assetname, 
            string account, string assigned_user_id)
        {
            this.product = product;
            this.serialnumber = serialnumber;
            this.datesold = datesold;
            this.dateinservice = dateinservice;
            this.assetstatus = assetstatus;
            this.assigned_user_id = assigned_user_id;
            this.assetname = assetname;
            this.account = account;
        }
        public string asset_no;
        public string product; //mandatory
        public string serialnumber; //mandatory
        public string datesold; //mandatory
        public string dateinservice; //mandatory
        public Assetstatus assetstatus; //mandatory
        public string tagnumber;
        public string invoiceid;
        public string shippingmethod;
        public string shippingtrackingnumber;
        public string assigned_user_id; //mandatory
        public string assetname; //mandatory
        public string account; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string description;
    }

    /// <summary>
    /// VTiger-ModComments object
    /// </summary>
    public class VTigerModComment : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.ModComments; }
        public VTigerModComment() { }
        public VTigerModComment(string commentcontent, string assigned_user_id, string related_to)
        {
            this.commentcontent = commentcontent;
            this.assigned_user_id = assigned_user_id;
            this.related_to = related_to;
        }
        public string commentcontent; //mandatory
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string related_to; //mandatory
        public string creator;
        public string parent_comments;
    }

    /// <summary>
    /// VTiger-ProjectMilestone object
    /// </summary>
    public class VTigerProjectMilestone : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.ProjectMilestone; }
        public VTigerProjectMilestone() { }
        public VTigerProjectMilestone(string projectmilestonename, string projectid, string assigned_user_id)
        {
            this.projectmilestonename = projectmilestonename;
            this.projectid = projectid;
            this.assigned_user_id = assigned_user_id;
        }
        public string projectmilestonename; //mandatory
        public string projectmilestonedate;
        public string projectid; //mandatory
        public Projectmilestonetype projectmilestonetype;
        public string assigned_user_id; //mandatory
        public string projectmilestone_no;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string description;
    }

    /// <summary>
    /// VTiger-ProjectTask object
    /// </summary>
    public class VTigerProjectTask : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.ProjectTask; }
        public VTigerProjectTask() { }
        public VTigerProjectTask(string projecttaskname, string projectid, string assigned_user_id)
        {
            this.projecttaskname = projecttaskname;
            this.projectid = projectid;
            this.assigned_user_id = assigned_user_id;
        }
        public string projecttaskname; //mandatory
        public Projecttasktype projecttasktype;
        public Projecttaskpriority projecttaskpriority;
        public string projectid; //mandatory
        public string assigned_user_id; //mandatory
        public int projecttasknumber;
        public string projecttask_no;
        public Projecttaskprogress projecttaskprogress;
        public string projecttaskhours;
        public string startdate;
        public string enddate;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string description;
    }

    /// <summary>
    /// VTiger-Project object
    /// </summary>
    public class VTigerProject : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Project; }
        public VTigerProject() { }
        public VTigerProject(string projectname, string assigned_user_id)
        {
            this.projectname = projectname;
            this.assigned_user_id = assigned_user_id;
        }
        public string projectname; //mandatory
        public string startdate;
        public string targetenddate;
        public string actualenddate;
        public Projectstatus projectstatus;
        public Projecttype projecttype;
        public string linktoaccountscontacts;
        public string assigned_user_id; //mandatory
        public string project_no;
        public string targetbudget;
        public string projecturl;
        public Projectpriority projectpriority;
        public Progress progress;
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string description;
    }

    /// <summary>
    /// VTiger-SMSNotifier object
    /// </summary>
    public class VTigerSMSNotifier : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.SMSNotifier; }
        public VTigerSMSNotifier() { }
        public VTigerSMSNotifier(string assigned_user_id, string message)
        {
            this.assigned_user_id = assigned_user_id;
            this.message = message;
        }
        public string assigned_user_id; //mandatory
        public DateTime createdtime;
        public DateTime modifiedtime;
        public string message; //mandatory
    }

    /// <summary>
    /// VTiger-Groups object
    /// </summary>
    public class VTigerGroup : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Groups; }
        public string groupname;
        public string description;
    }

    /// <summary>
    /// VTiger-Currency object
    /// </summary>
    public class VTigerCurrency : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.Currency; }
        public VTigerCurrency() { }
        public VTigerCurrency(string defaultid, int deleted)
        {
            this.defaultid = defaultid;
            this.deleted = deleted;
        }
        public string currency_name;
        public string currency_code;
        public string currency_symbol;
        public double conversion_rate;
        public string currency_status;
        public string defaultid; //mandatory
        public int deleted; //mandatory
    }

    /// <summary>
    /// VTiger-DocumentFolders object
    /// </summary>
    public class VTigerDocumentFolder : VTigerEntity
    {
        public override VTigerType GetElementType() { return VTigerType.DocumentFolders; }
        public VTigerDocumentFolder() { }
        public VTigerDocumentFolder(string foldername, string createdby)
        {
            this.foldername = foldername;
            this.createdby = createdby;
        }
        public string foldername; //mandatory
        public string description;
        public string createdby; //mandatory
        public int sequence;
    }

    #endregion

}
