using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.GameItems
{
    public class Card
    {
        public Card(Suits suit, CardValue value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        #region Public Properties

        public Suits Suit { get; set; }

        public CardValue Value { get; set; }

        public int IntValue
        {
            get
            {
                if(this.Value <= CardValue.Ten)
                {
                    return (int)this.Value;
                }
                else if(this.Value == CardValue.Ace)
                {
                    return 11;
                }
                else
                {
                    //Face Cards have a value of 10
                    return (int)CardValue.Ten;
                }
            }
        }

        #endregion

        #region Public Methods

        public string GetSuitString()
        {
            return Enum.GetName(typeof(Suits), this.Suit);
        }

        public string GetValueString()
        {
            return Enum.GetName(typeof(CardValue), this.Value);
        }

        public override string ToString()
        {
            return this.Value + " of " + this.Suit + "s";
        }

        #endregion

    }
}
