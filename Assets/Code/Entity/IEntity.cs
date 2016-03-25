using GamePlay;

namespace Entity {

	public interface IEntity {

        /*void addCardToDeck(string cardName);
		void addCardToHand (string cardName);
		void playCard (string cardName);
        void pickACard(int number);*/

        void addSkill(string skillName);
        void playSkill(string skillName);
        void affectEntity(BaseStats statsConcerned, int amount);

	}

}	