using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlignmentMethods{
        //Returns 1 if they are on the same side, -1 if they are on opposite side, returns 0 otherwise.
        public static int groupSide(this GroupData.Alignment mySide, GroupData.Alignment theirSide){
            //This is the easiest case to check since it's an enum if we they are both on the same side we return 1.
            if(mySide == theirSide && theirSide != GroupData.Alignment.None){
                return 1;
            }
            switch(mySide){
                //The following non-default casses capture the situation in which mySide is opposite to theirSide
                case GroupData.Alignment.Government:
                    if (theirSide==GroupData.Alignment.Communist){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Communist:
                    if (theirSide==GroupData.Alignment.Government){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Liberal:
                    if (theirSide==GroupData.Alignment.Conservative){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Conservative:
                    if (theirSide==GroupData.Alignment.Liberal){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Peaceful:
                    if (theirSide==GroupData.Alignment.Violent){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Violent:
                if (theirSide==GroupData.Alignment.Peaceful){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Straight:
                    if (theirSide==GroupData.Alignment.Weird){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Weird:
                    if (theirSide==GroupData.Alignment.Straight){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Criminal:
                    if (theirSide==GroupData.Alignment.Fanatic){
                        return -1;
                    }
                    return 0;
                case GroupData.Alignment.Fanatic:
                    if (theirSide==GroupData.Alignment.Criminal){
                        return -1;
                    }
                    return 0;
                //If they aren't on the same side or opposing sides we return 0.
                default:
                    return 0;
            }
        }
    }

static class AttackUtility {
 
    
    
    private static int PowerStructureBonus(GroupData defending){
        int masterCount = 0;
        GroupData node = defending;
        //We crawl up the GroupsMasters to determine which level in the powerstructure it is.
        while (node.Master!=null){
            node = node.Master;
            ++masterCount;
        }
        
        switch(masterCount){
            //One master means it's adjacent to the illuminati and gets + 10
            case 1:
                return 10;
                
            //Two masters means it gets + 5
            case 2:
                return 5;
                
            //Three masters means it gets + 2
            case 3:
                return 2;
                
            //Everything else doesn't efffect resitance.
            default:
                return 0;
        }
    }

    // Returns true if the attacking player successfully completes the attack and gains control of the group.
    public static int AttackToConquer(GroupData attacking, GroupData[] supporting, GroupData defending) {
        // Total power is the is the combined power of the attacking groups and it's supporters
        int totalPower = attacking.Power;
        
        //Entire for each can be commented if no supporting groups are going to be allowed
        //foreach (GroupData group in supporting){
        //    //Currently doesn't exist withing group but should according to standard Illuminati rules
        //    totalPower += group.Support;
        //    Debug.Log(group.Name + "Is supporting with:" + group.Support);
        //}
        // Adjust power if needed here for sub-attributes. (not implemented yet I don't understand how power can get affected)
        //totalPower += powerAdjust();


        //Resistance calculation
        //The defenders resitance
        int resists = defending.Resistance;
        resists += PowerStructureBonus(defending);
        GroupData.Alignment[] attackerAlignment = attacking.Alignments;
        GroupData.Alignment[] defenderAlignment = defending.Alignments;
        //Using the groupSide() method on the GroupData.alignments
        //We iterate over each attackerGroupData.Alignment, then we check each defenderGroupData.Alignment against the attackerGroupData.Alignment
        foreach (GroupData.Alignment aAlignment in attackerAlignment)
        {
            //For each defender GroupData.alignment if they are on opposing or the same side it will be -1 or 1 and we increment totalPower by 4 or -4
            //If the GroupData.Alignments are not related it returns 0 and totalPower doesn't change
            foreach(GroupData.Alignment dAlignment in defenderAlignment){
                totalPower+= aAlignment.groupSide(dAlignment)*4;
                if(aAlignment.groupSide(dAlignment) < 0){
                    Debug.Log(aAlignment + " opposes defender Alignment of: "+ dAlignment);
                }else if (aAlignment.groupSide(dAlignment) > 0){
                    Debug.Log(aAlignment + " is the same as defender Alignment of: "+ dAlignment);
                }
            }
        }
        Debug.Log("Total Power is: " + totalPower + "Defender Resistance: " + resists);
        int differential = totalPower - resists;
        Debug.Log("Must roll below: " + differential + " to conquer");


        // the following function is place holder to enter into a bidding war control flow:
        //biddingWar();

        return differential;
   
        









    }
    // Returns true if the defending group was destroyed
    //Pretty similar to the other attacks but opposite GroupData.alignments get bonuses and identical GroupData.alignments get penalties
    //Also, instead of rolling Power vs Resist, we roll Power vs Power
    public static bool AttackToDestroy(GroupData attacking, GroupData[] supporting, GroupData defending) {
        // Total power is the is the combined power of the attacking groups and it's supporters
        int totalPower = attacking.Power + 6;

        //Entire for each can be commented if no supporting groups are going to be allowed
        //foreach (var group in supporting){
        //    //Currently doesn't exist withing group but should according to standard Illuminati rules
        //    totalPower += attacking.Support;
        //}
        // Adjust power if needed here for sub-attributes. (not implemented yet I don't understand how power can get affected)
        //totalPower += powerAdjust();


        
        int resists = defending.Power;
        resists+=PowerStructureBonus(defending);

        //Consider making a function in GroupData that returns GroupData.alignment as an array would make life simpler here.
        GroupData.Alignment[] attackerAlignment = attacking.Alignments;
        GroupData.Alignment[] defenderAlignment = defending.Alignments;
        //Using the groupSide() method on the GroupData.alignments
        //We iterate over each attackerGroupData.Alignment, then we check each defenderGroupData.Alignment against the attackerGroupData.Alignment
        foreach (GroupData.Alignment aAlignment in attackerAlignment)
        {
            //For each defender GroupData.alignment if they are on opposing or the same side it will be 1 or -1 and we increment totalPower by 4 or -4
            //If the GroupData.Alignments are not related it returns 0 and totalPower doesn't change
            // We multiply by negative 4 to flip the bonuses and the penalties for Attacks to Destroy
            foreach(GroupData.Alignment dAlignment in defenderAlignment){
                totalPower+= aAlignment.groupSide(dAlignment)*-4;
            }
        }

        int differential = totalPower - resists;

        // the following function is place holder to enter into a bidding war control flow:
        //biddingWar();
        int roll = 4;  // diceRoll() should just return a random int from 1-6
        return roll<differential && roll < 11;
    }



    // Returns true if the defending group was neutralized
    // Attack is identical but the attacker gets a free + 6 bonus to their power
    public static bool AttackToNeutralize(GroupData attacking, GroupData[] supporting, GroupData defending) {
        // Total power is the is the combined power of the attacking groups and it's supporters
        int totalPower = attacking.Power + 6;

        //Entire for each can be commented if no supporting groups are going to be allowed
        //foreach (var group in supporting){
        //    //Currently doesn't exist withing group but should according to standard Illuminati rules
        //    totalPower += attacking.Support;
        //}
        // Adjust power if needed here for sub-attributes. (not implemented yet I don't understand how power can get affected)
        //totalPower += powerAdjust();


        //Are we still giving groups adjacency bonus on defense? This might need to be edited if that is the case.
        int resists = defending.Resistance;
        resists+= PowerStructureBonus(defending);

        //Consider making a function in GroupData that returns GroupData.alignment as an array would make life simpler here.
        GroupData.Alignment[] attackerAlignment = attacking.Alignments;
        GroupData.Alignment[] defenderAlignment = defending.Alignments;
        //Using the groupSide() method on the GroupData.alignments
        //We iterate over each attackerGroupData.Alignment, then we check each defenderGroupData.Alignment against the attackerGroupData.Alignment
        foreach (GroupData.Alignment aAlignment in attackerAlignment)
        {
            //For each defender GroupData.alignment if they are on opposing or the same side it will be 1 or -1 and we increment totalPower by 4 or -4
            //If the GroupData.Alignments are not related it returns 0 and totalPower doesn't change
            foreach(GroupData.Alignment dAlignment in defenderAlignment){
                totalPower+= aAlignment.groupSide(dAlignment)*4;
            }
        }

        int differential = totalPower - resists;

        // the following function is place holder to enter into a bidding war control flow:
        //biddingWar();


        int roll = 3;  // diceRoll() should just return a random int from 1-6


        return roll<differential && roll < 11;
    }

}


