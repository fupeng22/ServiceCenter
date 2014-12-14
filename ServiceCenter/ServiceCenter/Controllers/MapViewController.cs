using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SQLDAL;
using ServiceCenter.Filter;
using ServiceCenter.Models;

namespace ServiceCenter.Controllers
{
    [ErrorAttribute]
    public class MapViewController : Controller
    {
        //
        // GET: /MapView/

         [LoginValidate]
        public ActionResult Index(string MapId, string MapName, string GroupName, string MapPath)
        {
            ViewData["MapId"] = Server.UrlDecode(MapId).ToString();
            ViewData["MapName"] = Server.UrlDecode(MapName).ToString();
            ViewData["GroupName"] = Server.UrlDecode(GroupName).ToString();
            ViewData["MapPath"] = Server.UrlDecode(MapPath).ToString();

            return View();
        }

        [HttpGet]
        public string UpdateMapPot(string posX, string posY, string MapPotId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip1") + "\"}";

            string t_posX = Request.QueryString["posX"].ToString();
            string t_posY = Request.QueryString["posY"].ToString();
            string t_MapPotId = Request.QueryString["MapPotId"].ToString();

            try
            {
                if (new T_MapPots().UpdatePostion(Convert.ToDouble(t_posX), Convert.ToDouble(t_posY), Convert.ToInt32(t_MapPotId)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip2") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip3") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }

            return strRet;
        }

        [HttpGet]
        public string UpdateMapPotSize(string Width, string Height, string MapPotId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip4") + "\"}";

            string t_Width = Request.QueryString["Width"].ToString();
            string t_Height = Request.QueryString["Height"].ToString();
            string t_MapPotId = Request.QueryString["MapPotId"].ToString();

            try
            {
                if (new T_MapPots().UpdateSize(Convert.ToInt32(t_Width), Convert.ToInt32(t_Height), Convert.ToInt32(t_MapPotId)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip5") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip6") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }

            return strRet;
        }

        [HttpGet]
        public string GetAvailableEquipmentByGroupId(string hid_MapId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip7") + "\"}";

            string tmp_hid_MapId = Server.UrlDecode(Request.QueryString["hid_MapId"].ToString());
            strRet = new T_Equipment().LoadAvialableEquipmentByMapId(Convert.ToInt32(tmp_hid_MapId));
            return strRet;
        }

        [HttpGet]
        public string LoadMapPotDetail(string MapId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip8") + "\",\"data\":\"[]\"}";

            string tmp_MapId = Server.UrlDecode(Request.QueryString["MapId"].ToString());

            try
            {
                strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip9") + "\",\"data\":" + new T_MapPots().LoadMapPotDetailByMapId(Convert.ToInt32(tmp_MapId)) + "}";
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip10") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\",\"data\":\"[]\"}";
            }

            return strRet;
        }

        [HttpGet]
        public string LoadMapPotProperty(string MapPotId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip7") + "\"}";

            string tmp_MapPotId = Server.UrlDecode(Request.QueryString["MapPotId"].ToString());
            strRet = new T_MapPots().LoadMapPotDetailByPotId(Convert.ToInt32(tmp_MapPotId));
            return strRet;
        }

        [HttpGet]
        public string GetAvailableEquipmentByGroupId_Update(string hid_MapId, string EquipmentId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip7") + "\"}";

            string tmp_hid_MapId =Server.UrlDecode( Request.QueryString["hid_MapId"].ToString());
            string tmp_EquipmentId =Server.UrlDecode( Request.QueryString["EquipmentId"].ToString());
            strRet = new T_Equipment().LoadAvialableEquipmentByMapId_Update(Convert.ToInt32(tmp_hid_MapId), Convert.ToInt32(tmp_EquipmentId));
            return strRet;
        }

        [HttpGet]
        public string MapPotAdd(string MapEquipmentId, string MapId, string MapPotName, string posX, string posY, string Width, string Height)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip11") + "\"}";

            string tmp_MapEquipmentId = Server.UrlDecode(Request.QueryString["MapEquipmentId"].ToString());
            string tmp_MapId =Server.UrlDecode( Request.QueryString["MapId"].ToString());
            string tmp_MapPotName =Server.UrlDecode( Request.QueryString["MapPotName"].ToString());
            string tmp_posX =Server.UrlDecode( Request.QueryString["posX"].ToString());
            string tmp_posY = Server.UrlDecode(Request.QueryString["posY"].ToString());
            string tmp_Width =Server.UrlDecode( Request.QueryString["Width"].ToString());
            string tmp_Height = Server.UrlDecode(Request.QueryString["Height"].ToString());

            try
            {
                if (new T_MapPots().Add(Convert.ToInt32(tmp_MapId), Convert.ToInt32(tmp_MapEquipmentId), tmp_MapPotName, Convert.ToDouble(tmp_posX), Convert.ToDouble(tmp_posY), Convert.ToInt32(tmp_Width), Convert.ToInt32(tmp_Height)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip12") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip13") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string MapPotUpdate(string MapEquipmentId, string MapId, string MapPotName, string posX, string posY, string MapPotId, string Width, string Height)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip14") + "\"}";

            string tmp_MapEquipmentId =Server.UrlDecode( Request.QueryString["MapEquipmentId"].ToString());
            string tmp_MapId = Server.UrlDecode( Request.QueryString["MapId"].ToString());
            string tmp_MapPotName =Server.UrlDecode(  Request.QueryString["MapPotName"].ToString());
            string tmp_posX =Server.UrlDecode(  Request.QueryString["posX"].ToString());
            string tmp_posY = Server.UrlDecode( Request.QueryString["posY"].ToString());
            string tmp_MapPotId = Server.UrlDecode( Request.QueryString["MapPotId"].ToString());
            string tmp_Width =Server.UrlDecode(  Request.QueryString["Width"].ToString());
            string tmp_Height =Server.UrlDecode(  Request.QueryString["Height"].ToString());

            try
            {
                if (new T_MapPots().Update(Convert.ToInt32(tmp_MapId), Convert.ToInt32(tmp_MapEquipmentId), tmp_MapPotName, Convert.ToDouble(tmp_posX), Convert.ToDouble(tmp_posY), Convert.ToInt32(tmp_Width), Convert.ToInt32(tmp_Height), Convert.ToInt32(tmp_MapPotId)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip15") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip16") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }
            return strRet;
        }

        [HttpGet]
        public string MapPotDele(string mapPotId)
        {
            string strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip17") + "\"}";

            string tmp_mapPotId = Server.UrlDecode(Request.QueryString["mapPotId"].ToString());

            try
            {
                if (new T_MapPots().Dele(Convert.ToInt32(tmp_mapPotId)))
                {
                    strRet = "{\"result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip18") + "\"}";
                }
            }
            catch (Exception ex)
            {
                strRet = "{\"result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("MapView_Control_ErrorTip19") + ":" + ex.Message.Replace("'", "‘").Replace("\"", "“") + "\"}";
            }
            return strRet;
        }
    }
}
