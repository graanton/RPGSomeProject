using UnityEngine;

public interface IAttack
{
    public void StartAttacking();    
    public void StopAttacking();    
}

public interface IDirectionAttack: IAttack
{
    public void SetDirection(Vector3 direction);
}
