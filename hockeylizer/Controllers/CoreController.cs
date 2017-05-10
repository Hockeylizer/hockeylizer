﻿﻿﻿﻿﻿CreateTeam(CreateTeamVm vm) 
    - CreateTeamResult response

AddPlayer(AddPlayerVm vm)
    - AddPlayerResult response;

UpdatePlayerName(UpdateNameVm vm)
    - GeneralResult r;

DeletePlayer(DeletePlayerVm vm)
    - GeneralResult r

GetAllPlayers(GetAllPlayersVm vm)
    - GetPlayersResult response

CreateSession(CreateSessionVm vm)
    - SessionResult sr

AnalyzeThis(SessionVm vm)
    - GeneralResult response

public async Task<bool> AnalyzeSession(int sessionId)
    - bool

ChopThis(SessionVm vm)
    - GeneralResult response;

public async Task<bool> ChopSession(int sessionId)
    - bool

DeleteVideoFromSession(SessionVm vm)
    - GeneralResult response

IsAnalyzed(SessionVm vm)
    - IsAnalyzedResult response

IsChopped(SessionVm vm)
    - IsChoppedResult response

GetFramesFromShot(GetTargetFramesVm vm)
	- GetFramesFromShotResult response

GetDataFromShot(GetTargetFramesVm vm)
	- GetDataFromShotResult response;

UpdateTargetHit(UpdateTargetHitVm vm)
	- GeneralResult response;

GetFramesFromSession(SessionVm vm)
    - GetFramesResult response;

GetHitsOverviewSvg(int sessionId, string token)
    - string