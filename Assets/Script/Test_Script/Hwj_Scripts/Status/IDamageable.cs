
/*
    ㆍ IDamageable

    ㆍ 작성자 : 황원준

    ㆍ 기능 : 데미지를 입을 수 있는 모든 객체에 대한 공용 인터페이스
*/

public interface IDamageable
{
    void TakeDamage(float amount, UnityEngine.Vector3 attackerPosition, bool isBackAttack = false);
}