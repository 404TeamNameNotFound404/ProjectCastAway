using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    // ANIMATOR
    private Animator animator;


    [SerializeField] private Transform target;
    [SerializeField] private Transform boneIK;
    [SerializeField] private bool ikActive;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {       
        animator = GetComponent<Animator>();
        animator.animatePhysics = true;
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (ikActive)
    //    {
    //        if (target != null)
    //        {
    //            animator.SetLookAtWeight(1);
    //            animator.SetLookAtPosition(target.position); 
    //            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1); 
    //            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //            animator.SetIKPosition(AvatarIKGoal.RightHand, boneIK.position);
    //            animator.SetIKRotation(AvatarIKGoal.RightHand, boneIK.rotation);

    //        }
    //        //else
    //        //{
    //        //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
    //        //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    //        //    animator.SetLookAtWeight(0);

    //        //}
    //    }
    //}


    private void Update()
    {
        boneIK.position = target.position;
        boneIK.rotation = target.rotation;

        
    }
}
