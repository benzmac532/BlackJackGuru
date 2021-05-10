using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Participants
{
    public class Dealer : Player
    {
        public Dealer() : base()
        {
            
        }

        #region Public Methods

        public Card FaceUpCard
        {
            get { return this.PrimaryHand.FirstCard; }
        }

        public bool HasBlackjack
        {
            get { return this.PrimaryHand.HasBlackjack; }
        }

        public bool Busted
        {
            get { return this.PrimaryHand.Busted; }
        }

        public int Value
        {
            get { return this.PrimaryHand.Value; }
        }

        #endregion

    }
}
