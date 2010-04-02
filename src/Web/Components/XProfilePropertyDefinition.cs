// Author:             Joe Audette
// Created:            2006-10-29
// Last Modified:      2009-04-01

using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cynthia.Web.UI;
using Cynthia.Web.Framework;
using Cynthia.Web.Controls;
using Cynthia.Business;
using Cynthia.Business.WebHelpers;


namespace Cynthia.Web.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class CProfilePropertyDefinition
    {
        #region Constructors

        public CProfilePropertyDefinition()
        {

        }

        #endregion

        #region Private Properties

        private String name = String.Empty;
        private String type = "System.String";
        private string iSettingControlSrc = string.Empty;
        private bool includeTimeForDate = false; //applies only when type = System.DateTime
        private bool allowMarkup = false;
        private String resourceFile = "ProfileResource";
        private String labelResourceKey = String.Empty;
        private bool lazyLoad = false;
        private bool requiredForRegistration = false;
        private bool allowAnonymous = true;
        private bool visibleToAnonymous = false;
        private bool visibleToAuthenticated = true;
        private bool visibleToUser = true;
        private bool editableByUser = true;
        private String onlyAvailableForRoles = String.Empty;
        private String onlyVisibleForRoles = String.Empty;
        private bool includeHelpLink = false;
        private int maxLength = 0;
        private int rows = 0;
        private int columns = 0;
        private String regexValidationExpression = String.Empty;
        private String regexValidationErrorResourceKey = String.Empty;
        private string stateValue = string.Empty;

        private SettingsSerializeAs serializeAs = SettingsSerializeAs.String;
        private Collection<CProfilePropertyOption> optionList = new Collection<CProfilePropertyOption>();
        private String defaultValue = String.Empty;

        #endregion

        #region Public Properties

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String ISettingControlSrc
        {
            get { return iSettingControlSrc; }
            set { iSettingControlSrc = value; }
        }

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        public String StateValue
        {
            get { return stateValue; }
            set { stateValue = value; }
        }

        public bool IncludeTimeForDate
        {
            get { return includeTimeForDate; }
            set { includeTimeForDate = value; }
        }

        public bool AllowMarkup
        {
            get { return allowMarkup; }
            set { allowMarkup = value; }
        }

        public String ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

        public String LabelResourceKey
        {
            get { return labelResourceKey; }
            set { labelResourceKey = value; }
        }

        public bool LazyLoad
        {
            get { return lazyLoad; }
            set { lazyLoad = value; }
        }

        public bool RequiredForRegistration
        {
            get { return requiredForRegistration; }
            set { requiredForRegistration = value; }
        }

        public bool AllowAnonymous
        {
            get { return allowAnonymous; }
            set { allowAnonymous = value; }
        }

        public String OnlyAvailableForRoles
        {
            get { return onlyAvailableForRoles; }
            set { onlyAvailableForRoles = value; }
        }

        public String OnlyVisibleForRoles
        {
            get { return onlyVisibleForRoles; }
            set { onlyVisibleForRoles = value; }
        }

        public bool IncludeHelpLink
        {
            get { return includeHelpLink; }
            set { includeHelpLink = value; }
        }

        public bool VisibleToAnonymous
        {
            get { return visibleToAnonymous; }
            set { visibleToAnonymous = value; }
        }

        public bool VisibleToAuthenticated
        {
            get { return visibleToAuthenticated; }
            set { visibleToAuthenticated = value; }
        }

        public bool VisibleToUser
        {
            get { return visibleToUser; }
            set { visibleToUser = value; }
        }

        public bool EditableByUser
        {
            get { return editableByUser; }
            set { editableByUser = value; }
        }

        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public int Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        public String RegexValidationExpression
        {
            get { return regexValidationExpression; }
            set { regexValidationExpression = value; }
        }

        public String RegexValidationErrorResourceKey
        {
            get { return regexValidationErrorResourceKey; }
            set { regexValidationErrorResourceKey = value; }
        }

        public SettingsSerializeAs SerializeAs
        {
            get { return serializeAs; }
            set { serializeAs = value; }
        }

        public String DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public Collection<CProfilePropertyOption> OptionList
        {
            get
            {
                return optionList;
            }
        }

        #endregion

        #region UI Helper Methods

        public static void SetupPropertyControl(
            Page currentPage,
            Panel parentControl,
            CProfilePropertyDefinition propertyDefinition,
            Double timeZoneOffset,
            string siteRoot)
        {
            if (propertyDefinition.StateValue.Length > 0)
            {
                SetupPropertyControl(
                    currentPage,
                    parentControl,
                    propertyDefinition,
                    propertyDefinition.StateValue,
                    timeZoneOffset,
                    siteRoot);
            }
            else
            {
                SetupPropertyControl(
                    currentPage,
                    parentControl,
                    propertyDefinition,
                    propertyDefinition.DefaultValue,
                    timeZoneOffset,
                    siteRoot);

            }

        }

        
        public static void SetupPropertyControl(
            Page currentPage,
            Panel parentControl, 
            CProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double timeZoneOffset,
            string siteRoot)
        {
            if (propertyValue == null)
            {
                propertyValue = String.Empty;
            }

            Literal rowOpenTag = new Literal();
            rowOpenTag.Text = "<div class='settingrow'>";
            parentControl.Controls.Add(rowOpenTag);

            SiteLabel label = new SiteLabel();
            label.ResourceFile = propertyDefinition.ResourceFile;
            // if key isn't in resource file use assume the resource hasn't been
            //localized and just use the key as the resource
            label.ShowWarningOnMissingKey = false;
            label.ConfigKey = propertyDefinition.LabelResourceKey;
            label.CssClass = "settinglabel";

            if (propertyDefinition.ISettingControlSrc.Length > 0)
            {
                Control c = currentPage.LoadControl(propertyDefinition.ISettingControlSrc) ;

                if ((c != null)&&(c is ISettingControl))
                {
                    c.ID = "isc" + propertyDefinition.Name;
                    parentControl.Controls.Add(label);

                    ISettingControl settingControl = (ISettingControl)c;

                    settingControl.SetValue(propertyValue);
                    parentControl.Controls.Add(c);

                    if (propertyDefinition.IncludeHelpLink)
                    {
                        AddHelpLink(parentControl, propertyDefinition);
                    }
                }
            }
            else if (propertyDefinition.OptionList.Count > 0)
            {
                // add a dropdownlist with the options

                DropDownList dd = CreateDropDownQuestion(propertyDefinition, propertyValue);
                dd.ID = "dd" + propertyDefinition.Name;
                dd.EnableTheming = false;
                dd.CssClass = "forminput";
                
                dd.TabIndex = 10;
                label.ForControl = dd.ID;
                parentControl.Controls.Add(label);

                parentControl.Controls.Add(dd);

                if (propertyDefinition.IncludeHelpLink)
                {
                    AddHelpLink(parentControl, propertyDefinition);
                }

               
            }
            else
            {

                switch (propertyDefinition.Type)
                {
                    case "System.Boolean":
                        CheckBox checkBox = new CheckBox();
                        checkBox.TabIndex = 10;
                        checkBox.ID = "chk" + propertyDefinition.Name;
                        checkBox.CssClass = "forminput";
                        label.ForControl = checkBox.ID;
                        parentControl.Controls.Add(label);
                        parentControl.Controls.Add(checkBox);
                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyValue.ToLower() == "true")
                        {
                            checkBox.Checked = true;
                        }
                        break;

                    case "System.DateTime":
                        // TODO: to really make this culture aware we should store the users
                        // culture as well and use the user's culture to 
                        // parse the date
                        DatePickerControl datePicker = CreateDatePicker(propertyDefinition, propertyValue, timeZoneOffset, siteRoot);
                        
                        datePicker.TabIndex = 10;
                        datePicker.ID = "dp" + propertyDefinition.Name;
                        datePicker.CssClass = "forminput";
                        parentControl.Controls.Add(label);
                       
                        parentControl.Controls.Add(datePicker);
                        

                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyDefinition.RequiredForRegistration)
                        {
                            RequiredFieldValidator rfvDate = new RequiredFieldValidator();
                            rfvDate.ControlToValidate = datePicker.ID;

                            rfvDate.ErrorMessage = Resources.ProfileResource.RequiredLabel;
                            rfvDate.Display = ValidatorDisplay.Dynamic;
                            rfvDate.ValidationGroup = "profile";
                            parentControl.Controls.Add(rfvDate);
                        }

                        if (propertyDefinition.RegexValidationExpression.Length > 0)
                        {
                            RegularExpressionValidator regexValidatorDate = new RegularExpressionValidator();
                            regexValidatorDate.ControlToValidate = datePicker.ID;
                            regexValidatorDate.ValidationExpression = propertyDefinition.RegexValidationExpression;
                            regexValidatorDate.ValidationGroup = "profile";
                            if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
                            {
                                regexValidatorDate.ErrorMessage = ResourceHelper.GetResourceString(
                                    propertyDefinition.ResourceFile,
                                    propertyDefinition.RegexValidationErrorResourceKey);

                                //object o = HttpContext.GetGlobalResourceObject(
                                //    propertyDefinition.ResourceFile,
                                //    propertyDefinition.RegexValidationErrorResourceKey);

                                //if (o != null)
                                //{
                                //    regexValidatorDate.ErrorMessage = o.ToString();
                                //}
                                //else
                                //{
                                //    regexValidatorDate.ErrorMessage = propertyDefinition.RegexValidationErrorResourceKey;
                                //}

                            }

                            regexValidatorDate.Display = ValidatorDisplay.Dynamic;
                            parentControl.Controls.Add(regexValidatorDate);
                        }

                        break;

                    case "System.String":
                    default:

                        TextBox textBox = new TextBox();
                        textBox.TabIndex = 10;
                        textBox.ID = "txt" + propertyDefinition.Name;
                        textBox.CssClass = "forminput";
                        label.ForControl = textBox.ID;
                        parentControl.Controls.Add(label);
                  
                        if (propertyDefinition.MaxLength > 0)
                        {
                            textBox.MaxLength = propertyDefinition.MaxLength;
                        }

                        if (propertyDefinition.Columns > 0)
                        {
                            textBox.Columns = propertyDefinition.Columns;
                        }

                        if (propertyDefinition.Rows > 1)
                        {
                            textBox.TextMode = TextBoxMode.MultiLine;
                            textBox.Rows = propertyDefinition.Rows;
                        }

                        parentControl.Controls.Add(textBox);
                        if (propertyDefinition.IncludeHelpLink)
                        {
                            AddHelpLink(parentControl, propertyDefinition);
                        }

                        if (propertyValue.Length > 0)
                        {
                            textBox.Text = propertyValue;
                        }
                        if (propertyDefinition.RequiredForRegistration)
                        {
                            RequiredFieldValidator rfv = new RequiredFieldValidator();
                            rfv.ControlToValidate = textBox.ID;
                            
                            rfv.ErrorMessage = Resources.ProfileResource.RequiredLabel;
                            rfv.Display = ValidatorDisplay.Dynamic;
                            rfv.ValidationGroup = "profile";
                            parentControl.Controls.Add(rfv);
                        }

                        if (propertyDefinition.RegexValidationExpression.Length > 0)
                        {
                            RegularExpressionValidator regexValidator = new RegularExpressionValidator();
                            regexValidator.ControlToValidate = textBox.ID;
                            regexValidator.ValidationExpression = propertyDefinition.RegexValidationExpression;
                            regexValidator.ValidationGroup = "profile";
                            if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
                            {
                                regexValidator.ErrorMessage = ResourceHelper.GetResourceString(
                                    propertyDefinition.ResourceFile,
                                    propertyDefinition.RegexValidationErrorResourceKey);

                                //object o = HttpContext.GetGlobalResourceObject(
                                //    propertyDefinition.ResourceFile, 
                                //    propertyDefinition.RegexValidationErrorResourceKey);
                                
                                //if (o != null)
                                //{
                                //    regexValidator.ErrorMessage = o.ToString();
                                //}
                                //else
                                //{
                                //    regexValidator.ErrorMessage = propertyDefinition.RegexValidationErrorResourceKey;
                                //}

                            }

                            regexValidator.Display = ValidatorDisplay.Dynamic;
                            parentControl.Controls.Add(regexValidator);
                        }

                        break;

                }

            }

            
            Literal rowCloseTag = new Literal();
            rowCloseTag.Text = "</div>";
            parentControl.Controls.Add(rowCloseTag);

        }

        private static DropDownList CreateDropDownQuestion(
            CProfilePropertyDefinition propertyDefinition,
            String propertyValue)
        {
            DropDownList dd = new DropDownList();
            //dd.ID = "dd" + propertyDefinition.Name;
            //dd.TabIndex = 10;
            //label.ForControl = dd.ID;

            //parentControl.Controls.Add(dd);
            //if (propertyDefinition.IncludeHelpLink)
            //{
            //    AddHelpLink(parentControl, propertyDefinition, siteRoot);
            //}

            if (dd.Items.Count == 0)
            {
                foreach (CProfilePropertyOption option in propertyDefinition.OptionList)
                {
                    ListItem listItem = new ListItem();
                    listItem.Value = option.Value;
                    listItem.Text = option.TextResourceKey;
                    if (option.TextResourceKey.Length > 0)
                    {
                        if (HttpContext.Current != null)
                        {
                            Object obj = HttpContext.GetGlobalResourceObject(
                                propertyDefinition.ResourceFile, option.TextResourceKey);

                            if (obj != null)
                            {
                                listItem.Text = obj.ToString();
                            }
                        }
                    }

                    dd.Items.Add(listItem);
                }
            }

            ListItem defaultItem = dd.Items.FindByValue(propertyValue);
            if (defaultItem != null)
            {
                dd.ClearSelection();
                defaultItem.Selected = true;
            }

            return dd;


        }

        private static DatePickerControl CreateDatePicker(
            CProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double timeZoneOffset,
            string siteRoot)
        {
            DatePickerControl datePicker = new DatePickerControl();
            datePicker.ID = "dp" + propertyDefinition.Name;
  
            if (propertyValue.Length > 0)
            {
                DateTime dt;
                if (DateTime.TryParse(
                    propertyValue,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.AdjustToUniversal, out dt))
                {
                    
                    if (propertyDefinition.IncludeTimeForDate)
                    {
                        dt = dt.AddHours(timeZoneOffset);
                        datePicker.Text = dt.ToString();

                    }
                    else
                    {
                        datePicker.Text = dt.Date.ToShortDateString();
                    }
                }
                else
                {
                    datePicker.Text = propertyValue;
                }
            }
            else
            {
                if (propertyDefinition.DefaultValue.Length > 0)
                {
                    datePicker.Text = propertyDefinition.DefaultValue;
                }
            }


            datePicker.ShowTime = propertyDefinition.IncludeTimeForDate;

            return datePicker;

        }

        public static void SetupReadOnlyPropertyControl(
            Panel parentControl,
            CProfilePropertyDefinition propertyDefinition,
            String propertyValue,
            Double timeZoneOffset)
        {
            if (propertyValue == null)
            {
                propertyValue = String.Empty;
            }

            Literal rowOpenTag = new Literal();
            rowOpenTag.Text = "<div class='settingrow'>";
            parentControl.Controls.Add(rowOpenTag);

            SiteLabel label = new SiteLabel();
            label.ResourceFile = propertyDefinition.ResourceFile;
            // if key isn't in resource file use assume the resource hasn't been
            //localized and just use the key as the resource
            label.ShowWarningOnMissingKey = false;
            label.ConfigKey = propertyDefinition.LabelResourceKey;
            label.CssClass = "settinglabel";
            parentControl.Controls.Add(label);

            Label propertyLabel = new Label();
            parentControl.Controls.Add(propertyLabel);

            if (propertyDefinition.OptionList.Count > 0)
            {
                DropDownList dd = new DropDownList();
                dd.ID = "dd" + propertyDefinition.Name;

                foreach (CProfilePropertyOption option in propertyDefinition.OptionList)
                {
                    ListItem listItem = new ListItem();
                    listItem.Value = option.Value;
                    listItem.Text = option.TextResourceKey;
                    if (option.TextResourceKey.Length > 0)
                    {
                        if (HttpContext.Current != null)
                        {
                            Object obj = HttpContext.GetGlobalResourceObject(
                                propertyDefinition.ResourceFile, option.TextResourceKey);

                            if (obj != null)
                            {
                                listItem.Text = obj.ToString();
                            }
                        }
                    }

                    dd.Items.Add(listItem);
                }

                ListItem defaultItem = dd.Items.FindByValue(propertyValue);
                if (defaultItem != null)
                {
                    propertyLabel.Text = HttpUtility.HtmlEncode(defaultItem.Text);
                }
                //dd.Enabled = false;

            }
            else
            {
                switch (propertyDefinition.Type)
                {
                    
                    case "System.Boolean":
                        Literal litBool = new Literal();
                        SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                        litBool.Text = "<img src='" + siteSettings.SkinBaseUrl
                            + propertyValue.ToLower()
                            + ".png' alt='" + propertyDefinition.Name + "' />";

                        parentControl.Controls.Add(litBool);

                        break;

                    case "System.DateTime":
                        Literal litDateTime = new Literal();
                        DateTime dt;
                        if (DateTime.TryParse(
                            propertyValue,
                            CultureInfo.CurrentCulture,
                            DateTimeStyles.AdjustToUniversal, out dt))
                        {
                            
                            if (propertyDefinition.IncludeTimeForDate)
                            {
                                dt = dt.AddHours(timeZoneOffset);
                                litDateTime.Text = dt.ToString();
                            }
                            else
                            {
                                litDateTime.Text = dt.Date.ToShortDateString();
                            }
                        }
                        else
                        {
                            litDateTime.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
                        }

                        parentControl.Controls.Add(litDateTime);
                        break;

                    case "System.String":
                    default:

                        if (propertyValue.Length > 0)
                        {
                            if (propertyDefinition.AllowMarkup)
                            {
                                propertyLabel.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
                            }
                            else
                            {
                                if (propertyDefinition.Name.ToLower().IndexOf("url") > -1)
                                {
                                    Literal litLink = new Literal();
                                    litLink.Text = "<a href='" + HttpUtility.HtmlEncode(propertyValue)
                                        + "'>" + HttpUtility.HtmlEncode(propertyValue)
                                        + "</a>";

                                    parentControl.Controls.Add(litLink);

                                }
                                else
                                {
                                    propertyLabel.Text = HttpUtility.HtmlEncode(propertyValue);
                                }
                            }
                        }
                        else
                        {
                            propertyLabel.Text = "&nbsp;";
                        }
                        
                        break;

                }

            }

            if (propertyLabel.Text.Length > 0)
            {
                parentControl.Controls.Add(propertyLabel);
            }


            Literal rowCloseTag = new Literal();
            rowCloseTag.Text = "</div>";
            parentControl.Controls.Add(rowCloseTag);

        }

        private static void AddHelpLink(
            Panel parentControl,
            CProfilePropertyDefinition propertyDefinition)
        {
            Literal litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

            CHelpLink helpLinkButton = new CHelpLink();
            helpLinkButton.HelpKey = "profile-" + propertyDefinition.Name.ToLower() + "-help";
            parentControl.Controls.Add(helpLinkButton);

            litSpace = new Literal();
            litSpace.Text = "&nbsp;";
            parentControl.Controls.Add(litSpace);

        }

        public static void SaveProperty(
            SiteUser siteUser, 
            Panel parentControl, 
            CProfilePropertyDefinition propertyDefinition,
            Double timeZoneOffset)
        {
            String controlID;
            Control control;

            if (propertyDefinition.ISettingControlSrc.Length > 0)
            {
                controlID = "isc" + propertyDefinition.Name;
                control = parentControl.FindControl(controlID);
                if (control != null)
                {
                    siteUser.SetProperty(
                        propertyDefinition.Name,
                        ((ISettingControl)control).GetValue(),
                        propertyDefinition.SerializeAs,
                        propertyDefinition.LazyLoad);
                }

            }
            else
            {

                switch (propertyDefinition.Type)
                {
                    case "System.Boolean":

                        controlID = "chk" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            siteUser.SetProperty(
                                propertyDefinition.Name,
                                ((CheckBox)control).Checked,
                                propertyDefinition.SerializeAs,
                                propertyDefinition.LazyLoad);

                        }

                        break;

                    case "System.DateTime":

                        controlID = "dp" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            DatePickerControl dp = (DatePickerControl)control;
                            if (dp.Text.Length > 0)
                            {
                                DateTime dt;
                                if (DateTime.TryParse(
                                    dp.Text,
                                    CultureInfo.CurrentCulture,
                                    DateTimeStyles.AdjustToUniversal, out dt))
                                {
                                    
                                    if (propertyDefinition.IncludeTimeForDate)
                                    {
                                        dt = dt.AddHours(-timeZoneOffset);

                                        siteUser.SetProperty(
                                            propertyDefinition.Name,
                                            dt.ToString(),
                                            propertyDefinition.SerializeAs,
                                            propertyDefinition.LazyLoad);
                                    }
                                    else
                                    {
                                        siteUser.SetProperty(
                                            propertyDefinition.Name,
                                            dt.Date.ToShortDateString(),
                                            propertyDefinition.SerializeAs,
                                            propertyDefinition.LazyLoad);
                                    }

                                }
                                else
                                {
                                    siteUser.SetProperty(
                                    propertyDefinition.Name,
                                    dp.Text,
                                    propertyDefinition.SerializeAs,
                                    propertyDefinition.LazyLoad);
                                }

                            }
                            else
                            {

                                siteUser.SetProperty(
                                    propertyDefinition.Name,
                                    String.Empty,
                                    propertyDefinition.SerializeAs,
                                    propertyDefinition.LazyLoad);
                            }
                        }

                        break;

                    case "System.String":
                    default:

                        if (propertyDefinition.OptionList.Count > 0)
                        {
                            controlID = "dd" + propertyDefinition.Name;
                            control = parentControl.FindControl(controlID);
                            if (control != null)
                            {
                                if (control is DropDownList)
                                {
                                    DropDownList dd = (DropDownList)control;
                                    if (dd.SelectedIndex > -1)
                                    {
                                        siteUser.SetProperty(
                                            propertyDefinition.Name,
                                            dd.SelectedValue,
                                            propertyDefinition.SerializeAs,
                                            propertyDefinition.LazyLoad);
                                    }
                                }
                            }

                        }
                        else
                        {
                            controlID = "txt" + propertyDefinition.Name;
                            control = parentControl.FindControl(controlID);
                            if (control != null)
                            {
                                siteUser.SetProperty(
                                    propertyDefinition.Name,
                                    ((TextBox)control).Text,
                                    propertyDefinition.SerializeAs,
                                    propertyDefinition.LazyLoad);
                            }

                        }

                        break;

                }
            }

        }

        public static void SavePropertyDefault(
            SiteUser siteUser,
            CProfilePropertyDefinition propertyDefinition)
        {

            siteUser.SetProperty(
                            propertyDefinition.Name,
                            propertyDefinition.DefaultValue,
                            propertyDefinition.SerializeAs,
                            propertyDefinition.LazyLoad);


        }


        //public static void SaveProperty(
        //    SiteUser siteUser, 
        //    Panel parentControl, 
        //    CProfilePropertyDefinition propertyDefinition,
        //    Double timeZoneOffset)
        //{
        //    String controlID;
        //    Control control;

        //    switch (propertyDefinition.Type)
        //    {
        //        case "System.Boolean":

        //            controlID = "chk" + propertyDefinition.Name;
        //            control = parentControl.FindControl(controlID);
        //            if (control != null)
        //            {
        //                siteUser.SetProperty(
        //                    propertyDefinition.Name,
        //                    ((CheckBox)control).Checked,
        //                    propertyDefinition.SerializeAs,
        //                    propertyDefinition.LazyLoad);

        //            }

        //            break;

        //        case "System.DateTime":

        //            controlID = "dp" + propertyDefinition.Name;
        //            control = parentControl.FindControl(controlID);
        //            if (control != null)
        //            {
        //                DatePicker dp = (DatePicker)control;
        //                if (dp.Text.Length > 0)
        //                {
        //                    DateTime dt;
        //                    if (DateTime.TryParse(
        //                        dp.Text,
        //                        CultureInfo.CurrentCulture,
        //                        DateTimeStyles.AdjustToUniversal, out dt))
        //                    {
        //                        dt = dt.AddHours(-timeZoneOffset);
        //                        if (propertyDefinition.IncludeTimeForDate)
        //                        {
        //                            siteUser.SetProperty(
        //                                propertyDefinition.Name,
        //                                dt.ToString(),
        //                                propertyDefinition.SerializeAs,
        //                                propertyDefinition.LazyLoad);
        //                        }
        //                        else
        //                        {
        //                            siteUser.SetProperty(
        //                                propertyDefinition.Name,
        //                                dt.ToShortDateString(),
        //                                propertyDefinition.SerializeAs,
        //                                propertyDefinition.LazyLoad);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        siteUser.SetProperty(
        //                        propertyDefinition.Name,
        //                        dp.Text,
        //                        propertyDefinition.SerializeAs,
        //                        propertyDefinition.LazyLoad);
        //                    }

        //                }
        //                else
        //                {

        //                    siteUser.SetProperty(
        //                        propertyDefinition.Name,
        //                        String.Empty,
        //                        propertyDefinition.SerializeAs,
        //                        propertyDefinition.LazyLoad);
        //                }
        //            }

        //            break;

        //        case "System.String":
        //        default:

        //            if (propertyDefinition.OptionList.Count > 0)
        //            {
        //                controlID = "dd" + propertyDefinition.Name;
        //                control = parentControl.FindControl(controlID);
        //                if (control != null)
        //                {
        //                    if (control is DropDownList)
        //                    {
        //                        DropDownList dd = (DropDownList)control;
        //                        if (dd.SelectedIndex > -1)
        //                        {
        //                            siteUser.SetProperty(
        //                                propertyDefinition.Name,
        //                                dd.SelectedValue,
        //                                propertyDefinition.SerializeAs,
        //                                propertyDefinition.LazyLoad);
        //                        }
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                controlID = "txt" + propertyDefinition.Name;
        //                control = parentControl.FindControl(controlID);
        //                if (control != null)
        //                {
        //                    siteUser.SetProperty(
        //                        propertyDefinition.Name,
        //                        ((TextBox)control).Text,
        //                        propertyDefinition.SerializeAs,
        //                        propertyDefinition.LazyLoad);
        //                }

        //            }

        //            break;

        //    }

        //}

        //public static void SavePropertyDefault(
        //    SiteUser siteUser,
        //    CProfilePropertyDefinition propertyDefinition)
        //{

        //    siteUser.SetProperty(
        //                    propertyDefinition.Name,
        //                    propertyDefinition.DefaultValue,
        //                    propertyDefinition.SerializeAs,
        //                    propertyDefinition.LazyLoad);


        //}


        public static void LoadState(
            Panel parentControl,
            CProfilePropertyDefinition propertyDefinition)
        {
            String controlID;
            Control control;

            switch (propertyDefinition.Type)
            {
                case "System.Boolean":

                    controlID = "chk" + propertyDefinition.Name;
                    control = parentControl.FindControl(controlID);
                    if (control != null)
                    {
                        propertyDefinition.StateValue = ((CheckBox)control).Checked.ToString();
                        
                    }

                    break;

                case "System.DateTime":

                    controlID = "dp" + propertyDefinition.Name;
                    control = parentControl.FindControl(controlID);
                    if (control != null)
                    {
                        DatePickerControl dp = (DatePickerControl)control;
                        if (dp.Text.Length > 0)
                        {
                            propertyDefinition.StateValue = dp.Text;

                        }
                    }

                    break;

                case "System.String":
                default:

                    if (propertyDefinition.OptionList.Count > 0)
                    {
                        controlID = "dd" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            if (control is DropDownList)
                            {
                                DropDownList dd = (DropDownList)control;
                                if (dd.SelectedIndex > -1)
                                {
                                    propertyDefinition.StateValue = dd.SelectedValue;

                                }
                            }
                        }

                    }
                    else
                    {
                        controlID = "txt" + propertyDefinition.Name;
                        control = parentControl.FindControl(controlID);
                        if (control != null)
                        {
                            propertyDefinition.StateValue = ((TextBox)control).Text;
                            
                        }

                    }

                    break;

            }

        }

        #endregion



    }
}
