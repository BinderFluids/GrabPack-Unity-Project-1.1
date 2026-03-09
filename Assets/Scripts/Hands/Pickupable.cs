public class Pickupable : HandInteractable
{
    protected override void OnGrab(BaseHandBehaviour hand)
    {
        hand.GiveItem(this); 
    }
}