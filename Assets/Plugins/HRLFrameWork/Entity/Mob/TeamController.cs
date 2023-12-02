using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    public int teamId = 1;

    public void SetTeam(int _teamId)
    {
        teamId = _teamId;
    }

    public void SetTeam(TeamController otherTeamController)
    {
        teamId = otherTeamController.teamId;
    }

    public bool DetectTeam(int _teamId)
    {
        return (teamId == _teamId);
    }
}
