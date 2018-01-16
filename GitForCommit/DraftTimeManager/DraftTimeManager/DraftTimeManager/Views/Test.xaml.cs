using System;
using System.Collections.Generic;
using DraftTimeManager.Models;
using DraftTimeManager.Entities;

using Xamarin.Forms;
using System.Linq;

namespace DraftTimeManager.Views
{
    public partial class Test : ContentPage
    {
        public List<DraftResults> draftResult { get; set; }
        public Test()
        {
            InitializeComponent();

            using(var con = new ConnectionModel().CreateConnection())
            {
                draftResult = con.Table<DraftResults>().Select(x => new DraftResults
                {
                    Draft_Id = x.Draft_Id,
                    User_Id = x.User_Id,
                    Env_Id = x.Env_Id,
                    R1_Vs_User = x.R1_Vs_User,
                    R1_Result = x.R1_Result,
                    R2_Result = x.R2_Result,
                    R2_Vs_User = x.R2_Result,
                    R3_Result = x.R3_Result,
                    R3_Vs_User = x.R3_Result,
                    Rank = x.Rank,
                    Tournament_No = x.Tournament_No,
                    Pick_No = x.Pick_No,
                    Draft_Date = x.Draft_Date,

                }).ToList();
            }

            StackLayout stack = new StackLayout();
        }
    }
}
