﻿using PCRWeb.Models;
using PCRWeb.Service;
using PCRWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCRWeb.Controllers
{
    public class PrincessConnectController : Controller
    {
        // GET: PrincessConnect
        private readonly PrincessConnectDBService PrincessConnectDBService = new PrincessConnectDBService();
        public List<PrincessConnect> DataList { get; set; }
        static List<bool> pcrState = new List<bool>();//加static才能宣告程全域
        static PrincessConnectViewModel Data = new PrincessConnectViewModel();
        static List<String> allRoleList = new List<String>();
        static int total = 0;
        static int selectId = 0;
        public ActionResult Index(string Search, string s1)
        {
            var pcrList = new PrincessConnect[]{
            //--------------------------------------------------------------前衛----------------------------------------------------------------------
            new PrincessConnect{Name="莉馬"},new PrincessConnect{Name="宮子"},new PrincessConnect{Name="空花"},new PrincessConnect{Name="純"},
            new PrincessConnect{Name="空花(大江戶)"},new PrincessConnect{Name="香織"},new PrincessConnect{Name="紡希(萬聖節)"},new PrincessConnect{Name="怜(新年)"},
            new PrincessConnect{Name="凜"},new PrincessConnect{Name="貪吃佩可"},new PrincessConnect{Name="FES貪吃佩可"},new PrincessConnect{Name="流夏"},
            new PrincessConnect{Name="可可蘿(新年)"},new PrincessConnect{Name="望"},new PrincessConnect{Name="矛依未"},new PrincessConnect{Name="真琴"},
            new PrincessConnect{Name="嘉夜"},new PrincessConnect{Name="日和(新年)"},new PrincessConnect{Name="妮諾(大江戶)"},new PrincessConnect{Name="秋乃"},
            new PrincessConnect{Name="真琴(夏日)"},new PrincessConnect{Name="純(夏日)"},new PrincessConnect{Name="茉莉"},new PrincessConnect{Name="克蘿依"},
            new PrincessConnect{Name="茉莉(萬聖節)"},new PrincessConnect{Name="惠理子(情人節)"},new PrincessConnect{Name="秋乃(聖誕節)"},new PrincessConnect{Name="綾音(聖誕節)"},
            new PrincessConnect{Name="流夏(夏日)"},new PrincessConnect{Name="紡希"},new PrincessConnect{Name="祈梨"}, new PrincessConnect{Name="日和"},new PrincessConnect{Name="褉"},new PrincessConnect{Name="綾音"},
            new PrincessConnect{Name="褉(萬聖節)"},new PrincessConnect{Name="珠希"},new PrincessConnect{Name="智"},new PrincessConnect{Name="琪愛兒"},
            new PrincessConnect{Name="珠希(夏日)"},new PrincessConnect{Name="惠理子"},new PrincessConnect{Name="貪吃佩可(夏日)"},new PrincessConnect{Name="胡桃"},
            new PrincessConnect{Name="吉塔"},new PrincessConnect{Name="怜"},new PrincessConnect{Name="伊莉亞(聖誕節)"},new PrincessConnect{Name="杏奈(夏日)"},
            new PrincessConnect{Name="克莉絲提娜(聖誕節)"},new PrincessConnect{Name="靜流"},new PrincessConnect{Name="克莉絲提娜"},new PrincessConnect{Name="胡桃(聖誕節)"},
            //--------------------------------------------------------------中衛----------------------------------------------------------------------
            new PrincessConnect{Name="美美"},new PrincessConnect{Name="忍"},new PrincessConnect{Name="美美(萬聖節)"},new PrincessConnect{Name="卯月"},
            new PrincessConnect{Name="怜(萬聖節)"},new PrincessConnect{Name="靜流(情人節)"},new PrincessConnect{Name="真陽(遊俠)"},new PrincessConnect{Name="真陽"},new PrincessConnect{Name="智(魔法少女)"},
            new PrincessConnect{Name="優花梨"},new PrincessConnect{Name="優花梨(聖誕節)"},new PrincessConnect{Name="莫妮卡"},new PrincessConnect{Name="妮諾"},new PrincessConnect{Name="望(聖誕節)"},
            new PrincessConnect{Name="美冬"},new PrincessConnect{Name="鈴(遊俠)"},new PrincessConnect{Name="伊莉亞"},new PrincessConnect{Name="香織(夏日)"},
            new PrincessConnect{Name="咲戀"},new PrincessConnect{Name="杏奈"},new PrincessConnect{Name="忍(萬聖節)"},new PrincessConnect{Name="七七香(夏日)"},
            new PrincessConnect{Name="美冬(夏日)"}, new PrincessConnect{Name="可可蘿"},new PrincessConnect{Name="步未(奇幻)"},new PrincessConnect{Name="步未"},
            new PrincessConnect{Name="古蕾婭"},new PrincessConnect{Name="莫妮卡(魔法少女)"},new PrincessConnect{Name="茜里(天使)"},new PrincessConnect{Name="依里(天使)"},
            new PrincessConnect{Name="可可蘿(夏日)"},new PrincessConnect{Name="雷姆"},new PrincessConnect{Name="拉姆"},new PrincessConnect{Name="鈴"},
            new PrincessConnect{Name="可可蘿(公主)"},new PrincessConnect{Name="拉比林斯達"},new PrincessConnect{Name="深月"},new PrincessConnect{Name="初音(夏日)"},
            new PrincessConnect{Name="茜里"},new PrincessConnect{Name="依里"},new PrincessConnect{Name="咲戀(夏日)"},new PrincessConnect{Name="宮子(萬聖節)"}, 
            //--------------------------------------------------------------後衛-------------------------------------------------------------------------
            new PrincessConnect{Name="亞里莎"},new PrincessConnect{Name="安"},new PrincessConnect{Name="露"},new PrincessConnect{Name="似似花"},
            new PrincessConnect{Name="碧(插班生)"},new PrincessConnect{Name="凱留(新年)"},new PrincessConnect{Name="未央"},new PrincessConnect{Name="美里(夏日)"},
            new PrincessConnect{Name="璃乃"}, new PrincessConnect{Name="鈴奈"},new PrincessConnect{Name="鈴奈(夏日)"},new PrincessConnect{Name="栞"},
            new PrincessConnect{Name="栞(魔法少女)"},new PrincessConnect{Name="伊緒"},new PrincessConnect{Name="伊緒(夏日)"},new PrincessConnect{Name="鈴莓"},new PrincessConnect{Name="鈴莓(新年)"},
            new PrincessConnect{Name="愛蜜莉雅"},new PrincessConnect{Name="霞"},new PrincessConnect{Name="霞(魔法少女)"},new PrincessConnect{Name="璃乃(奇幻)"},new PrincessConnect{Name="美里"},
            new PrincessConnect{Name="七七香"}, new PrincessConnect{Name="優衣(新年)"},new PrincessConnect{Name="凱留"},new PrincessConnect{Name="初音"},new PrincessConnect{Name="美咲"},
            new PrincessConnect{Name="露娜"}, new PrincessConnect{Name="優衣(公主)"},new PrincessConnect{Name="千歌(聖誕節)"},new PrincessConnect{Name="鈴莓(夏日)"},
            new PrincessConnect{Name="凱留(夏日)"},new PrincessConnect{Name="碧"},new PrincessConnect{Name="千歌"},new PrincessConnect{Name="真步(夏日)"},new PrincessConnect{Name="真步"},
            new PrincessConnect{Name="優衣"},new PrincessConnect{Name="雪"},new PrincessConnect{Name="優妮"},new PrincessConnect{Name="鏡華"},new PrincessConnect{Name="美咲(萬聖節)"},new PrincessConnect{Name="鏡華(萬聖節)"}
            };//目前使用的
            string allName = "";
            for (int i = 0; i < 134; i++)
            {
                allRoleList.Add(pcrList[i].Name);
                pcrState.Add(true);
            }
            for (int i = 0; i < 134; i++)
            {
                if (pcrState[i] == true)
                {
                    allName += pcrList[i].Name + " ";
                }
            }
            ViewBag.List = allRoleList;
            ViewBag.pcrList = new SelectList(pcrList, "Name");
            ViewBag.pcrState = pcrState;
            ViewBag.total = total;
            ViewBag.allName = allName;
            Data.Search = Search;
            Data.DataList = PrincessConnectDBService.GetDataList(Data.Search);
            return View(Data);
        }
        public ActionResult HandleButtonClick(int stateId)//stateId就是button屬性的value
        {
            pcrState[stateId] = !pcrState[stateId];
            if (pcrState[stateId])
                total++;
            else
                total--;

            selectId = stateId;
            //return RedirectToAction("Index");
            return Redirect("Index");
        }
        public ActionResult HandleCreateButtonClick(int stateId)//stateId就是button屬性的value
        {
            pcrState[stateId] = !pcrState[stateId];
            if (pcrState[stateId])
                total++;
            else
                total--;

            selectId = stateId;
            //return RedirectToAction("Index");
            return Redirect("Create");
        }
        //新增留言一開始載入頁面
        public ActionResult Create()
        {
            var pcrList = new PrincessConnect[]{
            //--------------------------------------------------------------前衛----------------------------------------------------------------------
            new PrincessConnect{Name="莉馬"},new PrincessConnect{Name="宮子"},new PrincessConnect{Name="空花"},new PrincessConnect{Name="純"},
            new PrincessConnect{Name="空花(大江戶)"},new PrincessConnect{Name="香織"},new PrincessConnect{Name="紡希(萬聖節)"},new PrincessConnect{Name="怜(新年)"},
            new PrincessConnect{Name="凜"},new PrincessConnect{Name="貪吃佩可"},new PrincessConnect{Name="FES貪吃佩可"},new PrincessConnect{Name="流夏"},
            new PrincessConnect{Name="可可蘿(新年)"},new PrincessConnect{Name="望"},new PrincessConnect{Name="矛依未"},new PrincessConnect{Name="真琴"},
            new PrincessConnect{Name="嘉夜"},new PrincessConnect{Name="日和(新年)"},new PrincessConnect{Name="妮諾(大江戶)"},new PrincessConnect{Name="秋乃"},
            new PrincessConnect{Name="真琴(夏日)"},new PrincessConnect{Name="純(夏日)"},new PrincessConnect{Name="茉莉"},new PrincessConnect{Name="克蘿依"},
            new PrincessConnect{Name="茉莉(萬聖節)"},new PrincessConnect{Name="惠理子(情人節)"},new PrincessConnect{Name="秋乃(聖誕節)"},new PrincessConnect{Name="綾音(聖誕節)"},
            new PrincessConnect{Name="流夏(夏日)"},new PrincessConnect{Name="紡希"},new PrincessConnect{Name="祈梨"}, new PrincessConnect{Name="日和"},new PrincessConnect{Name="褉"},new PrincessConnect{Name="綾音"},
            new PrincessConnect{Name="褉(萬聖節)"},new PrincessConnect{Name="珠希"},new PrincessConnect{Name="智"},new PrincessConnect{Name="琪愛兒"},
            new PrincessConnect{Name="珠希(夏日)"},new PrincessConnect{Name="惠理子"},new PrincessConnect{Name="貪吃佩可(夏日)"},new PrincessConnect{Name="胡桃"},
            new PrincessConnect{Name="吉塔"},new PrincessConnect{Name="怜"},new PrincessConnect{Name="伊莉亞(聖誕節)"},new PrincessConnect{Name="杏奈(夏日)"},
            new PrincessConnect{Name="克莉絲提娜(聖誕節)"},new PrincessConnect{Name="靜流"},new PrincessConnect{Name="克莉絲提娜"},new PrincessConnect{Name="胡桃(聖誕節)"},
            //--------------------------------------------------------------中衛----------------------------------------------------------------------
            new PrincessConnect{Name="美美"},new PrincessConnect{Name="忍"},new PrincessConnect{Name="美美(萬聖節)"},new PrincessConnect{Name="卯月"},
            new PrincessConnect{Name="怜(萬聖節)"},new PrincessConnect{Name="靜流(情人節)"},new PrincessConnect{Name="真陽(遊俠)"},new PrincessConnect{Name="真陽"},new PrincessConnect{Name="智(魔法少女)"},
            new PrincessConnect{Name="優花梨"},new PrincessConnect{Name="優花梨(聖誕節)"},new PrincessConnect{Name="莫妮卡"},new PrincessConnect{Name="妮諾"},new PrincessConnect{Name="望(聖誕節)"},
            new PrincessConnect{Name="美冬"},new PrincessConnect{Name="鈴(遊俠)"},new PrincessConnect{Name="伊莉亞"},new PrincessConnect{Name="香織(夏日)"},
            new PrincessConnect{Name="咲戀"},new PrincessConnect{Name="杏奈"},new PrincessConnect{Name="忍(萬聖節)"},new PrincessConnect{Name="七七香(夏日)"},
            new PrincessConnect{Name="美冬(夏日)"}, new PrincessConnect{Name="可可蘿"},new PrincessConnect{Name="步未(奇幻)"},new PrincessConnect{Name="步未"},
            new PrincessConnect{Name="古蕾婭"},new PrincessConnect{Name="莫妮卡(魔法少女)"},new PrincessConnect{Name="茜里(天使)"},new PrincessConnect{Name="依里(天使)"},
            new PrincessConnect{Name="可可蘿(夏日)"},new PrincessConnect{Name="雷姆"},new PrincessConnect{Name="拉姆"},new PrincessConnect{Name="鈴"},
            new PrincessConnect{Name="可可蘿(公主)"},new PrincessConnect{Name="拉比林斯達"},new PrincessConnect{Name="深月"},new PrincessConnect{Name="初音(夏日)"},
            new PrincessConnect{Name="茜里"},new PrincessConnect{Name="依里"},new PrincessConnect{Name="咲戀(夏日)"},new PrincessConnect{Name="宮子(萬聖節)"}, 
            //--------------------------------------------------------------後衛-------------------------------------------------------------------------
            new PrincessConnect{Name="亞里莎"},new PrincessConnect{Name="安"},new PrincessConnect{Name="露"},new PrincessConnect{Name="似似花"},
            new PrincessConnect{Name="碧(插班生)"},new PrincessConnect{Name="凱留(新年)"},new PrincessConnect{Name="未央"},new PrincessConnect{Name="美里(夏日)"},
            new PrincessConnect{Name="璃乃"}, new PrincessConnect{Name="鈴奈"},new PrincessConnect{Name="鈴奈(夏日)"},new PrincessConnect{Name="栞"},
            new PrincessConnect{Name="栞(魔法少女)"},new PrincessConnect{Name="伊緒"},new PrincessConnect{Name="伊緒(夏日)"},new PrincessConnect{Name="鈴莓"},new PrincessConnect{Name="鈴莓(新年)"},
            new PrincessConnect{Name="愛蜜莉雅"},new PrincessConnect{Name="霞"},new PrincessConnect{Name="霞(魔法少女)"},new PrincessConnect{Name="璃乃(奇幻)"},new PrincessConnect{Name="美里"},
            new PrincessConnect{Name="七七香"}, new PrincessConnect{Name="優衣(新年)"},new PrincessConnect{Name="凱留"},new PrincessConnect{Name="初音"},new PrincessConnect{Name="美咲"},
            new PrincessConnect{Name="露娜"}, new PrincessConnect{Name="優衣(公主)"},new PrincessConnect{Name="千歌(聖誕節)"},new PrincessConnect{Name="鈴莓(夏日)"},
            new PrincessConnect{Name="凱留(夏日)"},new PrincessConnect{Name="碧"},new PrincessConnect{Name="千歌"},new PrincessConnect{Name="真步(夏日)"},new PrincessConnect{Name="真步"},
            new PrincessConnect{Name="優衣"},new PrincessConnect{Name="雪"},new PrincessConnect{Name="優妮"},new PrincessConnect{Name="鏡華"},new PrincessConnect{Name="美咲(萬聖節)"},new PrincessConnect{Name="鏡華(萬聖節)"}
            };//目前使用的
            string allName = "";
            for (int i = 0; i < 134; i++)
            {
                allRoleList.Add(pcrList[i].Name);
                pcrState.Add(false);
            }
            for (int i = 0; i < 134; i++)
            {
                if (pcrState[i] == true)
                {
                    allName += pcrList[i].Name + " ";
                }
            }
            ViewBag.List = allRoleList;
            ViewBag.pcrList = new SelectList(pcrList, "Name");
            ViewBag.pcrState = pcrState;
            ViewBag.total = total;
            ViewBag.allName = allName;
            return View();
        }
        [HttpPost]
        public ActionResult Create(PrincessConnectViewModel p)
        {
            PrincessConnectDBService.InsertPrincessConnectTen(p);
            if (ModelState.IsValid)
            {
                return RedirectToAction("Test");
            }
            return View();
        }

        #region 修改留言
        //修改留言頁面要根據傳入編號來決定要修改的資料
        public ActionResult Edit(int Id)
        {//取得頁面所需資料，藉由Service取得
            PrincessConnect Data = PrincessConnectDBService.GetDataById(Id);//取得頁面所需的資料
            return View(Data);//將資料傳入View中
        }
        [HttpPost]//修改留言傳入資料時的Action
        //設定此Action只接受頁面POST資料傳入
        //使用Bind的Include來定義只接受的欄位，用來避免傳入其他不相干值
        public ActionResult Edit(int Id, [Bind(Include = "Name,Content")] PrincessConnect UpdateData)
        {
            //修改資料的是否可修改判斷
            if (PrincessConnectDBService.CheckUpdate(Id))
            {
                UpdateData.Id = Id;//將編號設定至修改資料中
                PrincessConnectDBService.UpdatePrincessConnect(UpdateData);//使用Service來修改資料
            }
            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult Delete(int Id)
        {
            //使用Service來刪除資料
            PrincessConnectDBService.DeleteGuestbooks(Id);
            return RedirectToAction("Index");
        }
    }
}