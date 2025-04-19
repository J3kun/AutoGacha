
using System.Linq.Expressions;
using System.Runtime.InteropServices;

public class InvOwnerAutoGacha(Card owner = null, Card container = null, CurrencyType _currency = CurrencyType.None) : 
  InvOwnerGacha(owner, container, _currency)
{

  public override string langTransfer => "gacha";

  public override bool ShouldShowGuide(Thing t) => t.id == this.gacha.GetIdCoin();

  public override bool SingleTarget => false;

  public override InvOwnerDraglet.ProcessType processType => InvOwnerDraglet.ProcessType.Consume;

  public override void _OnProcess(Thing t)
  {
    SE.Play("gacha");
    this.gacha.PlayGacha(t.Num);
    t.Destroy();
  }
}