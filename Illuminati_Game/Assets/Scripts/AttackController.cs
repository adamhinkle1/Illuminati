using HyperCard;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class AttackController: MonoBehaviour
{

    [SerializeField] AttackType attack;
    Ray ray;
    RaycastHit hit;
    List<GroupData> supporters = new List<GroupData>();
    int count = 0;
    bool selected = false;
    GameObject objectTarget;
    private GameObject notifications;
    private enum AttackType
    {
        Control,Destroy,Neutralize
    }
    void Update()
    {
        if (selected)
        {
            Attack();
        }
        
      
        
    }

     void Start()
    {

    }

    private void Attack()
    {
     
        //Draws a ray from the camera to the where you mouse is.
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //If the ray hits something we store what it hits to hit
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            //The attacker
            //print(this.GetComponent<BoardCardDisplay>() + " Frame: "+count);
            ++count;
            GroupData attacker = this.GetComponent<BoardCardInterface>().GroupData;
            GroupData target;
            //We try to make what the RayCast hit into a GroupData if it didn't hit a Card it will error and we make it null
            try
            {
                objectTarget = GameObject.Find(hit.collider.name);
                target = GameObject.Find(hit.collider.name).GetComponent<BoardCardInterface>().GroupData;
                
                //print(target.Name);
            }
            catch (System.NullReferenceException e)
            {
                target = null;
                
            }

            //If we hit something that wasn't another card we break
            if (target == null)
            {
                DeselectCard();
                print("unselected due to hitting not a card");
                return;
            }
            //If the RayCast hit the attacker we select it.
            //print(target.ControllingPlayer);
            //if (target.Name == attacker.Name && Input.GetMouseButtonDown(0)){
            //    if(selected){
            //        //If we are unselecting the attacker we want to erase any supporters we might have had.
            //        supporters = new List<GroupData>();
            //    }
            //    //More code can go in here to make attacker glow and to bring up some kind of alert telling you how to proceed with the attack
            //    selected = !selected;
            //    print("The Attacker " + attacker.Name + "'s selection was toggled, it is now:" + selected);
            //If we already selected the attacker and the attacker isn't owned by the player we attack it
            /*}*/
            if (selected && Input.GetMouseButtonDown(0) && attacker.ControllingPlayer != target.ControllingPlayer)
            {
                selected = false;
                bool result;
                int tempResult;
                //The varius kinds of attack it could be
                switch (attack)
                {
                    case AttackType.Control:
                        print("in switch");
                        tempResult = AttackUtility.AttackToConquer(attacker, supporters.ToArray(), target);

                        GameObject dice1 = GameObject.Find("Dice1");
                        GameObject dice2 = GameObject.Find("Dice2");
                        GameObject dice = GameObject.Find("Dice");

                        dice1.GetComponent<Dice>().EnableDice();
                        dice2.GetComponent<Dice>().EnableDice();

                        StartCoroutine(dice.GetComponent<RollDice>().StartRoll(tempResult, dice, dice1, dice2, objectTarget));

                        
                        
                       
                        break;
                    case AttackType.Destroy:
                        result = AttackUtility.AttackToDestroy(attacker, supporters.ToArray(), target);
                        break;
                    case AttackType.Neutralize:
                        result = AttackUtility.AttackToNeutralize(attacker, supporters.ToArray(), target);
                        break;
                    default:
                        //This shouldn't happen the above cases are exhaustive but C# doesn't know that and I have to give it some value or it will complain.
                        result = false;
                        break;
                }

                
                return;

            }//If the attacker and the target are on the same side
            else if (selected && Input.GetMouseButtonDown(0) && attacker.ControllingPlayer == target.ControllingPlayer)
            {
                ////If the target isn't in the supporters list already we add it.
                //if(!supporters.Contains(target)){
                //    supporters.Add(target);
                //}//If the target is in the supportest list alread we remove it
                //else{
                //    supporters.Remove(target);
                //}
                notifications.GetComponentInChildren<TextMeshProUGUI>().text += "You can't attack your own side\n";

            }

            
        }
        else if (Input.GetMouseButtonDown(0))
        {
            DeselectCard();
        }

    }

    public void ContinueRoll(int tempResult, GameObject dice, GameObject dice1, GameObject dice2, GameObject target)
    {
        bool result;
        int roll = dice.GetComponent<RollDice>().Value;
        print("rolled: " + roll);
        result = (roll > tempResult && roll < 11);
        StartCoroutine(DisableDice(dice1, dice2));
        notifications.GetComponentInChildren<TextMeshProUGUI>().text += (result ? "You took control of " + target.GetComponent<BoardCardInterface>().GroupData.Name + "\n" : "You failed to take control of " + target.GetComponent<BoardCardInterface>().GroupData.Name + "\n");

        if (result)
        {
            //attackling uncontrolled group
            if (target.GetComponent<BoardCardInterface>().BoardCardStateSet == BoardCardInterface.BoardCardState.uncontrolled)
            {
                GameObject uncontrolledBoard = GameObject.Find("Uncontrolled Section");
                target.GetComponent<BoardCardInterface>().ControlBoardGroup();
            }

            GameObject powerStructure = GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.PowerStructure;
            //add to players power structure
            
           
            
            target.GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.PowerStructure.GetComponent<PowerStructure>().DropGroup(target);
            powerStructure.GetComponent<PowerStructure>().AddGroup(target);
            print("Finished dropping");
        }

        //if (GetComponent<BoardCardInterface>().GroupData.ControllingPlayer.NumControlledGroups > 3)
        //{
        //    GameObject  winM = GameObject.Find("Win Manager");

        //    winM.GetComponent<WinManager>().Win(gameObject);
        //}
        DeselectCard();
    }

    public void SelectCard()
    {
        GameObject notificationLocation = GameObject.FindGameObjectWithTag("Notification");
        notifications = (GameObject)Instantiate(Resources.Load("Notifications"));
        notifications.transform.SetParent(notificationLocation.transform);
        notifications.transform.position = notificationLocation.transform.position;
        notifications.GetComponentInChildren<TextMeshProUGUI>().text = "Select the group you wish to attack.\n";
        selected = true;
        
    }

    private void DeselectCard()
    {
        selected = false;

        GameObject[] boardCards = GameObject.FindGameObjectsWithTag("Board Card");
        foreach (GameObject boardCard in boardCards)
        {
            if (boardCard.tag != "Illuminati")
            {
                print(boardCard.GetComponent<BoardCardInterface>().GroupData.name + " unhighlighted");
                boardCard.GetComponent<Card>().Properties.IsOutlineEnabled = false;
                boardCard.GetComponent<Card>().Properties.FaceSide.Redraw();
                boardCard.GetComponent<BoardCardInterface>().AttackStateSet = BoardCardInterface.AttackState.neutral;
            }           
           
        }

        if (gameObject.tag == "Illuminati")
        {
            print(gameObject.GetComponent<BoardCardInterface>().GroupData.name + " unhighlighted");
            GetComponent<ParticleSystem>().Stop();
            GetComponent<BoardCardInterface>().AttackStateSet = BoardCardInterface.AttackState.neutral;
        }




        StartCoroutine(DestroyNotification());
    }

    public IEnumerator DestroyNotification()
    {
        yield return new WaitForSeconds(1f);
        Destroy(notifications);
    }

   public IEnumerator DisableDice(GameObject dice1, GameObject dice2)
    {
        yield return new WaitForSeconds(1.5f);
        dice1.GetComponent<Dice>().DisableDice();
        dice2.GetComponent<Dice>().DisableDice();
    }

    public void Win()
    {

    }
}