using System;

[Serializable]
public class Item //dont remember what this did
{
    public ItemType Type;
    public int Amount;

    public override string ToString()
    {
        return Type.ToString() + " " + Amount.ToString(); //Convert the nothings into readable crap.
    }
}

