//using UnityEngine;
//using GamePlay;
//using Entity;
//using Utility;
//using System;
//using System.Collections.Generic;

//namespace AI {
//    public class IAMethods : ScriptableObject {

//        private static List<GameObject> getPlayersClose(int range, GameObject caller) {
//            EntitySpawner entitySpawner = GameObject.Find("EntitySpawner").GetComponent<EntitySpawner>();
//            List<GameObject> closePlayer = new List<GameObject>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            foreach(int entityId in entitySpawner.groupPlayers.Keys) {
//                foreach(GameObject player in entitySpawner.groupPlayers[entityId]) {
//                    Vector2 playerPosition = new Vector2(player.transform.position.x / Constants.ESPACEMENT_CASE_X, player.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//                    if(Math.Abs(playerPosition.x - entityPosition.x) <= range && Math.Abs(playerPosition.y - entityPosition.y) <= range) {
//                        closePlayer.Add(player);
//                    }
//                }
//            }
//            return closePlayer;
//        }

//        private static List<GameObject> getAllPlayers(GameObject caller) {
//            EntitySpawner entitySpawner = GameObject.Find("EntitySpawner").GetComponent<EntitySpawner>();
//            List<GameObject> closePlayer = new List<GameObject>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            foreach (int entityId in entitySpawner.groupPlayers.Keys) {
//                foreach (GameObject player in entitySpawner.groupPlayers[entityId]) {
//                    closePlayer.Add(player);
//                }
//            }
//            return closePlayer;
//        }

//        private static List<GameObject> getAllAllies(GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            EntitySpawner entitySpawner = GameObject.Find("EntitySpawner").GetComponent<EntitySpawner>();
//            List<GameObject> closeAllies = new List<GameObject>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            foreach (GameObject player in entitySpawner.groupPlayers[entity.groupId]) {
//                closeAllies.Add(player);
//            }
//            return closeAllies;
//        }

//        [FunctionMethod("Is close")]
//        private static List<GameObject> getAlliesClose(int range, GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            EntitySpawner entitySpawner = GameObject.Find("EntitySpawner").GetComponent<EntitySpawner>();
//            List<GameObject> closeAlly = new List<GameObject>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            foreach (GameObject ally in entitySpawner.groupMobs[entity.groupId]) {
//                if(ally != caller) {
//                    Vector2 playerPosition = new Vector2(ally.transform.position.x / Constants.ESPACEMENT_CASE_X, ally.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//                    if (Math.Abs(playerPosition.x - entityPosition.x) <= range && Math.Abs(playerPosition.y - entityPosition.y) <= range) {
//                        closeAlly.Add(ally);
//                    }
//                }
//            }
//            return closeAlly;
//        }

//        #region Have type of Card

//        private static List<Card> haveOffensiveCard(GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            List<Card> cardCanPlay = new List<Card>();
//            foreach(Card card in entity.hand) {
//                if (card.category == CategoryCard.OFFENSIVE)
//                    cardCanPlay.Add(card);
//            }
//            return cardCanPlay;
//        }

//        private static List<Card> haveDefensiveCard(GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            List<Card> cardCanPlay = new List<Card>();
//            foreach (Card card in entity.hand) {
//                if (card.category == CategoryCard.DEFENSIVE)
//                    cardCanPlay.Add(card);
//            }
//            return cardCanPlay;
//        }

//        private static List<Card> haveMovementCard(GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            List<Card> cardCanPlay = new List<Card>();
//            foreach (Card card in entity.hand) {
//                if (card.category == CategoryCard.MOUVEMENT)
//                    cardCanPlay.Add(card);
//            }
//            return cardCanPlay;
//        }

//        private static List<Card> haveSupportCard(GameObject caller) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            List<Card> cardCanPlay = new List<Card>();
//            foreach (Card card in entity.hand) {
//                if (card.category == CategoryCard.SUPPORT)
//                    cardCanPlay.Add(card);
//            }
//            return cardCanPlay;
//        }

//        #endregion

//        #region Can play type of Card

//        private static void playACard(GameObject caller, List<GameObject> players, List<Card> cards) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            Dictionary<Card, List<GameObject>> distribution = new Dictionary<Card, List<GameObject>>();
//            for (int i = cards.Count - 1; i >= 0; i--) {
//                foreach (GameObject player in players) {
//                    Vector2 playerPosition = new Vector2(player.transform.position.x / Constants.ESPACEMENT_CASE_X, player.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//                    if (Math.Abs(playerPosition.x - entityPosition.x) <= cards[i].range && Math.Abs(playerPosition.y - entityPosition.y) <= cards[i].range) {
//                        if (!distribution.ContainsKey(cards[i]))
//                            distribution.Add(cards[i], new List<GameObject>());
//                        distribution[cards[i]].Add(player);
//                    }
//                }
//            }
//            if (distribution.Keys.Count > 0) {
//                System.Random r = new System.Random();
//                int number = r.Next(0, distribution.Keys.Count - 1);
//                int i = 0;
//                foreach (Card card in distribution.Keys) {
//                    if (i == number) {
//                        entity.cardNamePlaying = card.name;
//                        number = r.Next(0, distribution[card].Count - 1);
//                        card.doAction(distribution[card][number].transform.GetChild(0).gameObject, distribution[card][number].transform.GetChild(0).position);
//                        return;
//                    }
//                }
//            }
//        }

//        public static void playOffensiveCard(int range, GameObject caller) {
//            List<GameObject> closePlayer = IAMethods.getPlayersClose(range, caller);
//            if(closePlayer.Count > 0) {
//                List<Card> offensiveCards = IAMethods.haveOffensiveCard(caller);
//                if(offensiveCards.Count > 0) {
//                    playACard(caller, closePlayer, offensiveCards);
//                }
//            }
//        }

//        [FunctionMethod("Can play a")]
//        public static void playDefensiveCard(GameObject caller) {
//            List<Card> defensiveCards = IAMethods.haveDefensiveCard(caller);
//            if (defensiveCards.Count > 0) {
//                System.Random rand = new System.Random();
//                int number = rand.Next(0, defensiveCards.Count - 1);
//                defensiveCards[number].doAction(caller, caller.transform.position);
//            }
//        }

//        [FunctionMethod("Can play a")]
//        public static void playMovementCard(GameObject caller, bool isAggressive) {
//            List<Card> movementCards = IAMethods.haveMovementCard(caller);
//            if(movementCards.Count > 0) {
//                if(isAggressive)
//                    moveToClosestEnemy(caller, movementCards);
//                else {
//                    AEntity entity = caller.GetComponent<AEntity>();
//                    if (entity.statistique.healthPoint < entity.statistique.maxHealthPoint / 3 && entity.statistique.maxHealthPoint > 10)
//                        moveToClosestAlly(caller, movementCards);
//                    else
//                        moveToSafety();
//                }
//            }
//        }

//        [FunctionMethod("Can play a")]
//        public static bool canPlaySupportCard(int range, GameObject caller) {
//            List<GameObject> closeAlly = IAMethods.getAlliesClose(range, caller);
//            if(closeAlly.Count > 0) {
//                List<Card> supportCards = IAMethods.haveSupportCard(caller);
//                if(supportCards.Count > 0) {
//                    playACard(caller, closeAlly, supportCards);
//                }
//            }
//            return false;
//        }

//        #endregion

//        #region Move to
//        [FunctionMethod("Move to")]
//        public static void moveToSafety() {
//        }

//        [FunctionMethod("Move to")]
//        public static void moveToClosestEnemy(GameObject caller, List<Card> movementCards) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            GameObject closerEnnemy = null;
//            Vector2 closerEnnemyPosition = Vector2.zero;
//            List<GameObject> closeEnnemies = IAMethods.getAllPlayers(caller);
//            foreach(GameObject ennemy in closeEnnemies) {
//                Vector2 currentDifference = new Vector2(Math.Abs(ennemy.transform.position.x - entityPosition.x), Math.Abs(ennemy.transform.position.y - entityPosition.y));
//                if (closerEnnemy == null || (currentDifference.x < closerEnnemyPosition.x && currentDifference.y < closerEnnemyPosition.y)) {
//                    closerEnnemy = ennemy;
//                    closerEnnemyPosition = new Vector2(Math.Abs(ennemy.transform.position.x - entityPosition.x), Math.Abs(ennemy.transform.position.y - entityPosition.y));
//                }
//            }
//            if(closerEnnemy != null) {
//                for(int i = 0; i < movementCards.Count; i++) {
//                    if(movementCards[i].isInRange(closerEnnemy)) {
//                        movementCards[i].doAction(closerEnnemy, closerEnnemy.transform.position);
//                    }
//                }
//            }
//        }

//        [FunctionMethod("Move to")]
//        public static void moveToClosestAlly(GameObject caller, List<Card> movementCards) {
//            AEntity entity = caller.GetComponent<AEntity>();
//            Vector2 entityPosition = new Vector2(caller.transform.position.x / Constants.ESPACEMENT_CASE_X, caller.transform.position.z / Constants.ESPACEMENT_CASE_Z);
//            GameObject closeAlly = null;
//            Vector2 closerAllyPosition = Vector2.zero;
//            List<GameObject> closeAllies = IAMethods.getAllAllies(caller);
//            foreach (GameObject ennemy in closeAllies) {
//                Vector2 currentDifference = new Vector2(Math.Abs(ennemy.transform.position.x - entityPosition.x), Math.Abs(ennemy.transform.position.y - entityPosition.y));
//                if (closeAlly == null || (currentDifference.x < closerAllyPosition.x && currentDifference.y < closerAllyPosition.y)) {
//                    closeAlly = ennemy;
//                    closerAllyPosition = new Vector2(Math.Abs(ennemy.transform.position.x - entityPosition.x), Math.Abs(ennemy.transform.position.y - entityPosition.y));
//                }
//            }
//            if (closeAlly != null) {
//                for (int i = 0; i < movementCards.Count; i++) {
//                    if (movementCards[i].isInRange(closeAlly)) {
//                        if (entityPosition.x == closeAlly.transform.position.x) {
//                            if(entityPosition.y < closeAlly.transform.position.z)
//                                movementCards[i].doAction(closeAlly, new Vector3(closeAlly.transform.position.x, closeAlly.transform.position.y, closeAlly.transform.position.z - 1));
//                            else
//                                movementCards[i].doAction(closeAlly, new Vector3(closeAlly.transform.position.x, closeAlly.transform.position.y, closeAlly.transform.position.z + 1));
//                        }
//                        else {
//                            if (entityPosition.x < closeAlly.transform.position.x)
//                                movementCards[i].doAction(closeAlly, new Vector3(closeAlly.transform.position.x - 1, closeAlly.transform.position.y, closeAlly.transform.position.z));
//                            else
//                                movementCards[i].doAction(closeAlly, new Vector3(closeAlly.transform.position.x + 1, closeAlly.transform.position.y, closeAlly.transform.position.z));
//                        }
//                    }
//                }
//            }
//        }
//        #endregion

//    }
//}