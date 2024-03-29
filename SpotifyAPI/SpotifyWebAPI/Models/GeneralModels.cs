﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpotifyAPI.SpotifyWebAPI.Models
{
    public class Image
    {
        [JsonProperty("url")]
        public String Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }
    public class Error
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("message")]
        public String Message { get; set; }
    }
    public class PlaylistTrackCollection
    {
        [JsonProperty("href")]
        public String Href { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
    public class Followers
    {
        [JsonProperty("href")]
        public String Href { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
    }
    public class PlaylistTrack
    {
        [JsonProperty("added_at")]
        public DateTime AddedAt { get; set; }
        [JsonProperty("added_by")]
        public PublicProfile AddedBy { get; set; }
        [JsonProperty("track")]
        public FullTrack Track { get; set; }
    }
    internal class CreatePlaylistArgs
    {
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("public")]
        public Boolean Public { get; set; }
    }
    public class SeveralTracks
    {
        [JsonProperty("tracks")]
        public List<FullTrack> Tracks { get; set; }
    }
    public class SeveralArtists
    {
        [JsonProperty("artists")]
        public List<FullArtist> Artists { get; set; }
    }
    public class SeveralAlbums
    {
        [JsonProperty("albums")]
        public List<FullAlbum> Albums { get; set; }
    }
}
