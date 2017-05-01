﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;


namespace hockeylizer.Models
{
    public class UploadFileResult
    {
        public UploadFileResult()
        {
            this.FilePath = "";
            this.FileName = "";
        }

        public UploadFileResult(string fp, string fn)
        {
            this.FilePath = fp;
            this.FileName = fn;
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }
    }

    public class UpdateNameVm
    {
        public bool Validate() 
        {
            if (playerId == null)
            {
                this.Result = new GeneralResult(false, "SpelarId är inte med i anropet.");
                return false;
            }

            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                this.Result = new GeneralResult(false, "Det nya namnet får inte vara tomt.");
                return false;
            }

            this.Result = new GeneralResult(true, "Allt fixat!");
            return true;
        }

        public int? playerId { get; set; }
        public string name { get; set; }
        public string token { get; set; }

        public GeneralResult Result { get; set; }
    }

    public class DeletePlayerVm
    {
        public bool Validate()
        {
            if (playerId == null)
            {
                this.Result = new GeneralResult(false, "SpelarId är inte med i anropet.");
                return false;
            }

            this.Result = new GeneralResult(true, "Allt fixat!");
            return true;
        }

        public int? playerId { get; set; }
        public string token { get; set; }

        public GeneralResult Result { get; set; }
    }

    public class GeneralResult
    {
        public GeneralResult()
        {
            this.Completed = false;
            this.Description = "Okänt fel";
        }

        public GeneralResult(bool comp, string desc)
        {
            this.Completed = comp;
            this.Description = desc;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

	public class CreateTeamResult
	{
		public CreateTeamResult()
		{
			this.Completed = false;
			this.Description = "Okänt fel";
		}

		public CreateTeamResult(bool comp, string desc)
		{
			this.Completed = comp;
			this.Description = desc;
		}

		public bool Completed { get; set; }
		public string Description { get; set; }

        public Guid? TeamId { get; set; }
	}

    public class UploadVideoVm
    {
        public bool Validate() 
        {
            var allowedFileTypes = "msvideo ogg mp4 avi mpeg quicktime 3gpp 3gpp2";
            var currentFileType = this.video.ContentType.Split('/').LastOrDefault();

            if (!allowedFileTypes.Contains(currentFileType))
            {
                this.vr = new VideoResult("Videoklippet kunde inte laddas upp då videon har fel feltyp! Kan ej ta emot filer av typ: " + currentFileType + ". Endast " + allowedFileTypes + " tas emot.", false);
                return false;
            }

            if (this.video == null || this.video.Length == 0)
			{
				this.vr = new VideoResult("Videoklippet kunde inte laddas upp då videon saknas!", false);
                return false;
			}

			if (this.interval == null)
			{
				this.vr = new VideoResult("Videoklippet kunde inte laddas upp då intervall saknas!", false);
                return false;
			}

			if (this.rounds == null)
			{
				this.vr = new VideoResult("Videoklippet kunde inte laddas upp då rundor saknas!", false);
                return false;
			}

			if (this.numberOfTargets == null)
			{
				this.vr = new VideoResult("Videoklippet kunde inte laddas upp då antal skott saknas!", false);
                return false;
			}

            if (!this.timestamps.Any())
            {
                this.vr = new VideoResult("Videoklippet kunde inte laddas upp då timestamps saknas!", false);
                return false;
            }

			if (!this.targetOrder.Any())
			{
				this.vr = new VideoResult("Videoklippet kunde inte laddas upp då skottordning saknas!", false);
                return false;
			}


            var to = this.targetOrder.Count;
            var tc = this.targetCoords.Count;
            var ts = this.timestamps.Count;

            if (!(to == tc && to == ts && tc == ts))
            {
                this.vr = new VideoResult("Videoklippet kunde inte laddas upp då antalet listor inte stämmer överens!", false);
                return false;
            }

            this.vr = new VideoResult("Videoklippet laddades upp!", true);
			return true;
        }

        public int? playerId { get; set; }
        public IFormFile video { get; set; }
        public int? interval { get; set; }
        public int? rounds { get; set; }
        public int? shots { get; set; }
        public int? numberOfTargets { get; set; }
        public List<ShotTimestampVm> timestamps { get; set; }
        public List<int> targetOrder { get; set; }
        public List<TargetCoordsVm> targetCoords { get; set; }
        public string token { get; set; }

        public VideoResult vr { get; set; }
    }

    public class ShotTimestampVm
    {
        public long Start { get; set; }
        public long End { get; set; }
    }

    public class AddPlayerResult
    {
        public AddPlayerResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public AddPlayerResult(string desc, bool comp, int? pl = null)
        {
            this.PlayerId = pl;
            this.Description = desc;
            this.Completed = comp;
        }

        public int? PlayerId { get; set; }
        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class PlayerVmSmall
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }

    public class GetPlayersResult
    {
        public GetPlayersResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Players = new List<PlayerVmSmall>();
        }

        public GetPlayersResult(bool comp, string desc, List<PlayerVmSmall> ps)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Players = ps;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<PlayerVmSmall> Players { get; set; }
    }

    public class VideoVmSmall
    {
        public int VideoId { get; set; }
        public string VideoPath { get; set; }

        public int? Interval { get; set; }
        public int? Rounds { get; set; }
        public int? Shots { get; set; }
        public int? NumberOfTargets { get; set; }

        public List<Target> Targets { get; set; }
    }

    public class GetVideosResult
    {
        public GetVideosResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Videos = new List<VideoVmSmall>();
        }

        public GetVideosResult(bool comp, string desc, List<VideoVmSmall> vs)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Videos = vs;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<VideoVmSmall> Videos { get; set; }
    }

    public class GetFramesResult
    {
        public GetFramesResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Images = new List<string>();
        }

        public GetFramesResult(bool comp, string desc, List<string> imgs)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Images = imgs;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }

    public class VideoResult
    {
        public VideoResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public VideoResult(string desc, bool comp, int? videoId = null)
        {
            this.Description = desc;
            this.Completed = comp;
            this.VideoId = videoId;
        }

        public int? VideoId { get; set; }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class TargetCoordsVm
    {
        public int? xCoord { get; set; }
        public int? yCoord { get; set; }
    }
}
