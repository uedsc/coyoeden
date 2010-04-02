/// Author:				    Joe Audette
/// Created:			    2005-04-10
/// Last Modified:		    2009-01-25
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using Cynthia.Business;
using Cynthia.Web.Controls.google;
using Cynthia.Web.Framework;
using Resources;

namespace Cynthia.Web.EventCalendarUI
{
	
    public partial class EventCalendarViewEvent : CBasePage
	{
        
		private int moduleId = -1;
        private int itemId = -1;
        private Hashtable moduleSettings;
        protected string GmapApiKey = string.Empty;
        protected int GoogleMapHeightSetting = 300;
        protected int GoogleMapWidthSetting = 500;
        protected bool GoogleMapEnableMapTypeSetting = false;
        protected bool GoogleMapEnableZoomSetting = false;
        protected bool GoogleMapShowInfoWindowSetting = false;
        protected bool GoogleMapEnableLocalSearchSetting = false;
        protected bool GoogleMapEnableDirectionsSetting = false;
        protected MapType mapType = MapType.G_NORMAL_MAP;
        private int GoogleMapInitialZoomSetting = 13;


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        private void Page_Load(object sender, System.EventArgs e)
		{
            LoadSettings();

            if (!UserCanViewPage())
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateControls();
			
		}

        private void PopulateControls()
        {
            if (itemId > -1)
            {
                CalendarEvent calendarEvent = new CalendarEvent(itemId);
                this.lblTitle.Text = calendarEvent.Title;
                this.litDescription.Text = calendarEvent.Description;
                this.lblDate.Text = calendarEvent.EventDate.ToShortDateString();
                this.lblStartTime.Text = calendarEvent.StartTime.ToShortTimeString();
                this.lblEndTime.Text = calendarEvent.EndTime.ToShortTimeString();

                if (calendarEvent.Location.Length > 0)
                {
                    gmap.GMapApiKey = GmapApiKey;
                    gmap.Location = calendarEvent.Location;
                    gmap.EnableMapType = GoogleMapEnableMapTypeSetting;
                    gmap.EnableZoom = GoogleMapEnableZoomSetting;
                    gmap.ShowInfoWindow = GoogleMapShowInfoWindowSetting;
                    gmap.EnableLocalSearch = GoogleMapEnableLocalSearchSetting;
                    gmap.MapHeight = GoogleMapHeightSetting;
                    gmap.MapWidth = GoogleMapWidthSetting;
                    gmap.EnableDrivingDirections = GoogleMapEnableDirectionsSetting;
                    gmap.GmapType = mapType;
                    gmap.ZoomLevel = GoogleMapInitialZoomSetting;
                    gmap.DirectionsButtonText = EventCalResources.DrivingDirectionsButton;
                    gmap.DirectionsButtonToolTip = EventCalResources.DrivingDirectionsButton;
                    
                }
                else
                {
                    gmap.Visible = false;
                }

            }

        }

        private void LoadSettings()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            GmapApiKey = SiteUtils.GetGmapApiKey();

            if (moduleSettings.Contains("GoogleMapInitialMapTypeSetting"))
            {
                string gmType = moduleSettings["GoogleMapInitialMapTypeSetting"].ToString();
                try
                {
                    mapType = (MapType)Enum.Parse(typeof(MapType), gmType);
                }
                catch (ArgumentException) { }

            }

            GoogleMapHeightSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GoogleMapHeightSetting", GoogleMapHeightSetting);

            GoogleMapWidthSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GoogleMapWidthSetting", GoogleMapWidthSetting);

            GoogleMapInitialZoomSetting = WebUtils.ParseInt32FromHashtable(
                moduleSettings, "GoogleMapInitialZoomSetting", GoogleMapInitialZoomSetting);


            GoogleMapEnableMapTypeSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GoogleMapEnableMapTypeSetting", false);

            GoogleMapEnableZoomSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GoogleMapEnableZoomSetting", false);

            GoogleMapShowInfoWindowSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GoogleMapShowInfoWindowSetting", false);

            GoogleMapEnableLocalSearchSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GoogleMapEnableLocalSearchSetting", false);

            GoogleMapEnableDirectionsSetting = WebUtils.ParseBoolFromHashtable(
                moduleSettings, "GoogleMapEnableDirectionsSetting", false);

            
        }

		
	}
}
