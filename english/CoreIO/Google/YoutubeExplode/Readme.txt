    // Get data
    Video = await _client.GetVideoAsync(videoId);
    Channel = await _client.GetVideoAuthorChannelAsync(videoId);
    MediaStreamInfos = await _client.GetVideoMediaStreamInfosAsync(videoId);
    ClosedCaptionTrackInfos = await _client.GetVideoClosedCaptionTrackInfosAsync(videoId); 