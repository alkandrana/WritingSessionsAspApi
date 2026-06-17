namespace WritingSessionsAspApi.Models;

public class PlotVM
{
    public int Sequence { get; set; }
    public string Name { get; set; }
    public int WordCount { get; set; }
    public int TSF { get; set; }
    public double POT  { get; set; }

    public string Beat  { get; set; }

    public void SetBeat()
    {
        if (POT < 12)
        {
            Beat = "Exposition";
        }
        else if (POT < 20)
        {
            Beat = "Inciting Incident";
        } else if (POT < 30)
        {
            Beat = "Threshold";
        }
        else if (POT < 40)
        {
            Beat = "Rising Action";
        }
        else if (POT < 50)
        {
            Beat = "Approach";
        }
        else if (POT < 70)
        {
            Beat = "Midpoint";
        }
        else if (POT < 80)
        {
            Beat = "Dark Night";
        }
        else if (POT < 95)
        {
            Beat = "Climax";
        }
        else
        {
            Beat = "Denouement";
        }
        // if (POT > 0 && POT < 8)
        // {
        //     Beat = "Really Bad Day";
        // }
        // else if (POT >= 8 && POT < 12)
        // {
        //     Beat = "Something Peculiar";
        // }
        // else if (POT >= 12 && POT < 16)
        // {
        //     Beat = "Grasping at Straws";
        // }
        // else if (POT >= 16 && POT < 20)
        // {
        //     Beat = "Call of Adventure";
        // }
        // else if (POT >= 20 && POT < 24)
        // {
        //     Beat = "Head in Sand";
        // }
        // else if (POT >= 24 && POT < 28)
        // {
        //     Beat = "Pull Out Rug";
        // }
        // else if (POT >= 28 && POT < 32)
        // {
        //     Beat = "Enemies and Allies";
        // }
        // else if (POT >= 32 && POT < 36)
        // {
        //     Beat = "Games and Trials";
        // }
        // else if (POT >= 36 && POT < 40)
        // {
        //     Beat = "Earning Respect";
        // }
        // else if (POT >= 40 && POT < 44)
        // {
        //     Beat = "Forces of Evil";
        // }
        // else if (POT >= 44 && POT < 48)
        // {
        //     Beat = "Problem Revealed";
        // }
        // else if (POT >= 48 && POT < 52)
        // {
        //     Beat = "Truth and Ultimatum";
        // }
        // else if (POT >= 52 && POT < 56)
        // {
        //     Beat = "Mirror Stage";
        // }
        // else if (POT >= 56 && POT < 60)
        // {
        //     Beat = "Plan of Attack";
        // }
        // else if (POT >= 60 && POT < 64)
        // {
        //     Beat = "Crucial Role";
        // }
        // else if (POT >= 64 && POT < 68)
        // {
        //     Beat = "Direct Conflict";
        // }
        // else if (POT >= 68 && POT < 72)
        // {
        //     Beat = "Surprise Failure";
        // }
        // else if (POT >= 72 && POT < 76)
        // {
        //     Beat = "Shocking Revelation";
        // }
        // else if (POT >= 76 && POT < 80)
        // {
        //     Beat = "Giving Up";
        // }
        // else if (POT >= 80 && POT < 84)
        // {
        //     Beat = "Pep Talk";
        // }
        // else if (POT >= 84 && POT < 88)
        // {
        //     Beat = "Seizing the Sword";
        // } 
        // else if (POT >= 88 && POT < 92)
        // {
        //     Beat = "Ultimate Defeat";
        // }
        // else if (POT >= 92 && POT < 96)
        // {
        //     Beat = "Unexpected Victory";
        // }
        // else if (POT >= 96 && POT < 100)
        // {
        //     Beat = "Bittersweet Reflection";
        // }
    }
}