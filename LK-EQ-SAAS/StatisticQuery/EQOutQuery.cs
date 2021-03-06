﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YtPlugin;
using YtUtil.tool;
using ChSys;
using YiTian.db;
using YtWinContrl.com.datagrid;
using YtClient;

namespace StatisticQuery
{
    public partial class EQOutQuery : Form, IPlug
    {
        public EQOutQuery()
        {
            InitializeComponent();
        }
        #region IPlug 成员

        public Form getMainForm()
        {
            return this;
        }
        private void init()
        {

        }
        public void initPlug(IAppContent app, object[] param)
        {

        }

        public bool unLoad()
        {
            return true;
        }
        //  private Panel[] plis = null;

        #endregion
        
        void dateTimeDuan1_SelectChange(object sender, EventArgs e)
        {
            Search_button_Click(null, null);
        }
     

        private void Search_button_Click(object sender, EventArgs e)
        {
            if (this.selTextInpt2.Value == null && this.selTextInpt_KS.Value == null)
            {
                WJs.alert("请选择出库库房或者科室");

                return;
            }

            SqlStr sql = new SqlStr();


            if (!this.selTextInpt2.Text.Equals(""))
            {
                sql.Add("and a.WARECODE=?", this.selTextInpt2.Value);
            }
            if (!this.selTextInpt_KS.Text.Equals(""))
            {
                sql.Add("and a.DEPTID=?", this.selTextInpt_KS.Value);
            }
            if (!this.selTextInpt1.Text.Equals(""))
            {
                sql.Add("and a.IOID=?", this.selTextInpt1.Value);
            }

            this.dataGView1.reLoad(new object[] { His.his.Choscode, this.dateTimePicker1.Value, this.dateTimePicker2.Value }, sql);

            this.TiaoSu.Text = this.dataGView1.RowCount.ToString() + "笔";
            this.JinEHeJi.Text = this.dataGView1.Sum("总金额").ToString() + "元";
            this.label7.Text = this.dataGView1.Sum("零售总金额").ToString() + "元";

        }
        private void dataGView1_SelectionChanged(object sender, EventArgs e)
        {
            Dictionary<string, ObjItem> dr = this.dataGView1.getRowData();
            if (dr != null)
            {

                this.dataGView2.reLoad(new object[] { dr["出库ID"].ToString(), His.his.Choscode });
               

            }
            else
            {
                this.dataGView2.reLoad(null);
            }

            this.label18.Text = this.dataGView2.RowCount.ToString() + "条";
            this.label14.Text = this.dataGView2.Sum("金额").ToString() + "元";
            this.label9.Text = this.dataGView2.Sum("运杂费").ToString() + "元";
            this.label11.Text = this.dataGView2.Sum("成本金额").ToString() + "元";
        }

        private void EQOutQuery_Load(object sender, EventArgs e)
        {
            TvList.newBind().Load("FindKSName_EQOutQuery", new object[] { His.his.Choscode }).Bind(this.Column8);
            TvList.newBind().Load("FindWareName_EQOutQuery", new object[] { His.his.Choscode }).Bind(this.Column47);

            WJs.SetDictTimeOut();
            this.dataGView1.Url = "StatistQuery_EQOutMainSearch";
            this.dataGView2.Url = "StatistQuery_EQOutDetailSearch";
            this.dateTimeDuan1.InitCorl();
            this.dateTimeDuan1.SelectedIndex = -1;
            this.dateTimePicker1.Value = DateTime.Now.AddMonths(-1);
            this.selTextInpt2.Sql = "StaticQuery_EQOutWare";

            this.selTextInpt2.SelParam = His.his.Choscode + "|{key}|{key}|{key}|{key}";
            TvList.newBind().add("等待审核", "1").add("作废", "0").add("已审核", "2").add("已出库", "6").Bind(this.Column7);

            this.selTextInpt1.Sql = "StatistQuery_EQGetOutWareWay";
            this.selTextInpt1.SelParam = His.his.Choscode + "|{key}|{key}|{key}|{key}";

            this.selTextInpt_KS.Sql = "StatQuery_EQOutQueryKS";
            this.selTextInpt_KS.SelParam = His.his.Choscode + "|{key}|{key}|{key}";

            this.dateTimeDuan1.SelectChange += new EventHandler(dateTimeDuan1_SelectChange);
            this.dataGView1.SelectionChanged +=new EventHandler(dataGView1_SelectionChanged);
           
       
        }

      

       
    }
}
