using System;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;
using DraftTimeManager.Models;
using DraftTimeManager.Entities;

namespace DraftTimeManager.Views
{
    public partial class MatchTableCreate : ContentPage
    {
        private MatchTableModel model;

        public MatchTableCreate(bool randomFlg)
        {

            InitializeComponent();

            model = new MatchTableModel(randomFlg);

            createView();
        }

        private void createView()
        {
            model.createPairing();

            matchButtonCreate();
        }

        private void matchButtonCreate()
        {
            if(model.PlayerList.Where(m => m.R3_Result==null).Count()==0)
            {
                Navigation.PushAsync(new DraftResult((List<DraftResults>)model.PlayerList.ToList()),true);
                return;
            }
            else if(model.PlayerList.Count() == 4 && model.PlayerList.Where(m => m.R2_Result == null).Count() == 0)
            {
                Navigation.PushAsync(new DraftResult((List<DraftResults>)model.PlayerList.ToList()), true);
                return;
            }

            List<string> usedList = new List<string>();
            List<MatchInfo> matchInfo = model.MatchButtonCreate();

            var grid = new Grid
            {
                RowDefinitions = {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                },
            };

            foreach (var match in matchInfo.Select((p, i) => new { Content = p, Index = i }))
            {
                string bye = "";

                if(match.Content.PlayerA_ID == 9)
                {
                    bye = $"ROUND {match.Content.CurrentRound} {Environment.NewLine} {match.Content.PlayerB_Name} is Bye";
                }
                else if(match.Content.PlayerB_ID == 9)
                {
                    bye = $"ROUND {match.Content.CurrentRound} {Environment.NewLine} {match.Content.PlayerA_Name} is Bye";
                }

                var label = new Label()
                {
                    Text = bye.Equals("") ? (match.Content.buttonEnable? $"ROUND {match.Content.CurrentRound} {Environment.NewLine} {match.Content.PlayerA_Name}{Environment.NewLine} VS{Environment.NewLine} {match.Content.PlayerB_Name}"
                                :match.Content.CurrentRound !=3?
                                             "Please wait until the next opponent is decided.":"Please wait until all matches are over."):bye,
                    VerticalTextAlignment = TextAlignment.Center,
                    IsEnabled = match.Content.buttonEnable,
                    BackgroundColor = match.Content.ButtonColor,
                    FontSize = 24,
                };

                var tgr = new TapGestureRecognizer();
                tgr.Tapped += async (s, e) =>
                {
                    //DisplayActionSheetの表示
                    var result = await DisplayActionSheet("Winner is", "Cancel", null, $"{match.Content.PlayerA_Name}", $"{match.Content.PlayerB_Name}");
                    //返された文字列でボタンテキストを書き換える

                    if(result.Equals(match.Content.PlayerA_Name))
                    {
                        model.MatchResultUpdate(match.Content.PlayerA_ID, 1);
                        model.MatchResultUpdate(match.Content.PlayerB_ID, 0);
                        createView();
                    }
                    else if(result.Equals(match.Content.PlayerB_Name))
                    {
                        model.MatchResultUpdate(match.Content.PlayerB_ID, 1);
                        model.MatchResultUpdate(match.Content.PlayerA_ID, 0);
                        createView();
                    }
                    else
                    {
                        return;
                    }
                };

                label.GestureRecognizers.Add(tgr);

                grid.Children.Add(label, 0, match.Index);

            }

            this.Content = grid;
        }
    }
}
