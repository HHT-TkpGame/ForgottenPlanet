using UnityEngine;

public class StarTrailController : MonoBehaviour
{
    [SerializeField] AudioSource se;
    [SerializeField] Camera cam;
    ParticleSystem particle;
    ParticleSystem.MainModule main;
    ParticleSystem.TrailModule trail;
    const float MAX_FOV = 120f;
    const float FOV_CHANGE_SPEED = 2f * 40f;
    const float START_SPEED = 0.2f;
    const float START_TRAIL = 0f;
    const float MAX_SPEED = 30f;
    const float MAX_TRAIL = 0.2f;

    bool isHyperDriving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        Init();
    }

    void Init()
    {
        main = particle.main;
        trail = particle.trails;
        SetSpeedAndTrail(START_SPEED, START_TRAIL);
    }
    public void TransitionToHyperDrive()
    {
        se.Play();
        isHyperDriving = true;
        SetSpeedAndTrail(MAX_SPEED, MAX_TRAIL);
    }
    void SetSpeedAndTrail(float speed, float trail)
    {
        this.main.startSpeed = speed;
        this.trail.lifetime = trail;
    }
    int count;
    // Update is called once per frame
    void Update()
    {
        if (isHyperDriving)
        {
            if (cam.fieldOfView < MAX_FOV)
            {
                cam.fieldOfView += FOV_CHANGE_SPEED*Time.deltaTime;
            }
        }
    }
}
