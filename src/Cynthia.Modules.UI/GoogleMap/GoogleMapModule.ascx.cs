/// Author:					Joe Audette
/// Created:				2007-12-05
/// Last Modified:			2009-11-12
/// ApplicationGuid:		7E6407CF-A287-4461-AC74-5C05CFDE8999
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.

using System;
using Cynthia.Web.Controls.google;
using Cynthia.Web.Framework;
using Resources;



namespace Cynthia.Web.MapUI
{
    
    public partial class GoogleMapModule : SiteModuleControl
    {
        private int mapHeight = 300;
        private int mapWidth = 500;

        private bool enableMapType = false;
        private bool enableZoom = false;
        private bool showInfoWindow = false;
        private bool enableLocalSearch = false;
        private bool useLocationAsTitle = false;
        private bool useLocationAsCaption = false;
        private bool enableDrivingDirections = false;
        private MapType mapType = MapType.G_NORMAL_MAP;
        private int GoogleMapInitialZoomSetting = 13;


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            TitleControl.EditUrl = SiteRoot + "/Admin/ModuleSettings.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;

            if (Settings.Contains("GoogleMapLocationSetting"))
            {
                gmap.Location = Settings["GoogleMapLocationSetting"].ToString();
            }

            // if this is not set then it uses the one from Web.config
            if (Settings.Contains("GoogleMapApiKeySetting"))
            {
                string googleApiKey = Settings["GoogleMapApiKeySetting"].ToString();
                if (googleApiKey.Length > 10)
                {
                    gmap.GMapApiKey = googleApiKey;
                }
            }


            if (this.ModuleConfiguration != null)
            {
                if (useLocationAsTitle)
                {
                    this.ModuleConfiguration.ModuleTitle = gmap.Location;
                }

                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            
            

            gmap.EnableMapType = enableMapType;
            gmap.EnableZoom = enableZoom;
            gmap.ShowInfoWindow = showInfoWindow;
            gmap.EnableLocalSearch = enableLocalSearch;
            gmap.MapHeight = mapHeight;
            gmap.MapWidth = mapWidth;
            gmap.EnableDrivingDirections = enableDrivingDirections;
            gmap.GmapType = mapType;
            gmap.ZoomLevel = GoogleMapInitialZoomSetting;
            gmap.NoApiKeyWarning = GMapResources.NoApiKeyWarning;
            
            if (Settings.Contains("GoogleMapCaptionSetting"))
            {
                litCaption.Text = Settings["GoogleMapCaptionSetting"].ToString() + "<br />";
            }

            if (useLocationAsCaption)
            {
                litCaption.Text = Server.HtmlEncode(gmap.Location) + "<br />";
            }

        }


        private void PopulateLabels()
        {
            TitleControl.EditText = GMapResources.GoogleMapEditLink;
            TitleControl.ToolTip = GMapResources.GoogleMapEditLinkToolTip;
            gmap.DirectionsButtonText = GMapResources.GoogleMapGetDirectionsFromButton;
            gmap.DirectionsButtonToolTip = GMapResources.GoogleMapGetDirectionsFromButtonToolTip;
        }

        private void LoadSettings()
        {
           
            enableMapType = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableMapTypeSetting", enableMapType);

            enableZoom = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableZoomSetting", enableZoom);

            showInfoWindow = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapShowInfoWindowSetting", showInfoWindow);

            enableLocalSearch = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableLocalSearchSetting", enableLocalSearch);

            enableDrivingDirections = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapEnableDirectionsSetting", enableDrivingDirections);

            useLocationAsTitle = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapUseLocationForTitleSetting", useLocationAsTitle);

            useLocationAsCaption = WebUtils.ParseBoolFromHashtable(
                Settings, "GoogleMapShowLocationCaptionSetting", useLocationAsCaption);

            mapHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapHeightSetting", 300);

            mapWidth = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapWidthSetting", 500);

            GoogleMapInitialZoomSetting = WebUtils.ParseInt32FromHashtable(
                Settings, "GoogleMapInitialZoomSetting", GoogleMapInitialZoomSetting);

            

            gmap.GMapApiKey = SiteUtils.GetGmapApiKey();

            if (Settings.Contains("GoogleMapInitialMapTypeSetting"))
            {
                string gmType = Settings["GoogleMapInitialMapTypeSetting"].ToString();
                try
                {
                    mapType = (MapType)Enum.Parse(typeof(MapType), gmType);
                }
                catch (ArgumentException) { }

            }


        }


    }
}