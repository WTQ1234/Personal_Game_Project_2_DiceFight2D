using HRL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Combat
{
    public class SubView_RoundOver : SubView
    {
        public Text txt_Player1_Score;
        public Text txt_Player2_Score;
        protected override void Init_Components()
        {
            txt_Player1_Score = views["Txt_Player_1_Score"].GetComponent<Text>();
            txt_Player2_Score = views["Txt_Player_2_Score"].GetComponent<Text>();
        }
        
        protected override void Init_ComponentsValue()
        {
            Close();
        }

        /// <summary>
        /// 写入玩家分数Text
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public void SetScore(int player1, int player2)
        {
            txt_Player1_Score.text = player1.ToString();
            txt_Player2_Score.text = player2.ToString();
        }
    }
}