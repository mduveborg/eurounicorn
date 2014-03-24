'use strict';

angular.module('frontendAppDev', ['frontendApp', 'ngMockE2E'])
	.run(function ($httpBackend) {

		function regEsc(str) {
			return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\-\\\^\$\|]/g, '\\$&');
		}
		// Pass through request for views and images
		$httpBackend.whenGET( new RegExp( regEsc( 'views/' ) ) ).passThrough();
		$httpBackend.whenGET( new RegExp( regEsc( 'images/' ) ) ).passThrough();

		//override this route with a response
		$httpBackend.whenPOST('/api/login').respond({ token: 'dev' });

		$httpBackend.whenGET('/api/submissions').passThrough();

		$httpBackend.whenGET('/api/submissions/list').respond([{"id":139523902,"title":"The Sign of the Unicorn","soundCloudMeta":{"state":"finished","streamUrl":"https://api.soundcloud.com/tracks/139523902/stream","downloadUrl":"https://api.soundcloud.com/tracks/139523902/download","playbackCount":37,"duration":176132,"createdAt":"2014/03/14 11:09:05 +0000","downloadable":"False"},"customTrackMeta":{"trackId":139523902,"username":"johan.bjurling@netlight.com","songTitle":null,"stageName":"Headhunters","musicians":"Christian Dahlgren, Andreas Persson, Johan Bjurling","composers":"Johan Bjurling","docType":"track","_id":"ca2497384bcda10060a6e2f057aad1a4","_rev":"1-2efdb5c7bc6418c9ea1f7dc51f94f14e"},"embed":"\u003ciframe width=\"100%\" height=\"300\" scrolling=\"no\" frameborder=\"no\" src=\"https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/139523902%3Fsecret_token%3Ds-K3jxo&amp;auto_play=false&amp;hide_related=false&amp;visual=true\"\u003e\u003c/iframe\u003e"},{"id":138384027,"title":"The Ballad of the Unicorns - Humming mix","soundCloudMeta":{"state":"finished","streamUrl":"https://api.soundcloud.com/tracks/138384027/stream","downloadUrl":"https://api.soundcloud.com/tracks/138384027/download","playbackCount":106,"duration":240139,"createdAt":"2014/03/07 14:05:07 +0000","downloadable":"True"},"customTrackMeta":{"trackId":138384027,"username":"johan.blomgren@netlight.com","songTitle":null,"stageName":"Purple Unicorn","musicians":"Johan Bjurling, Christian Dahlgren, Andreas Persson","composers":"Christian Dahlgren","docType":"track","_id":"8d61db3d5aa5fb5200b82ef035bab74f","_rev":"1-c789c593dfc7f4760464b521171f0009"},"embed":"\u003ciframe width=\"100%\" height=\"300\" scrolling=\"no\" frameborder=\"no\" src=\"https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/138384027%3Fsecret_token%3Ds-d82BY&amp;auto_play=false&amp;hide_related=false&amp;visual=true\"\u003e\u003c/iframe\u003e"},{"id":138383853,"title":"The Ballad of the Unicorns - Instrumental mix","soundCloudMeta":{"state":"finished","streamUrl":"https://api.soundcloud.com/tracks/138383853/stream","downloadUrl":"https://api.soundcloud.com/tracks/138383853/download","playbackCount":128,"duration":240139,"createdAt":"2014/03/07 14:03:43 +0000","downloadable":"True"},"customTrackMeta":{"trackId":138383853,"username":"johan.blomgren@netlight.com","songTitle":null,"stageName":"Purple Unicorn","musicians":"Johan Bjurling, Christian Dahlgren, Andreas Persson","composers":"Christian Dahlgren","docType":"track","_id":"8d61db3d5aa5fb5200b82ef035b89154","_rev":"1-20d201b7843be3b84370ddf8aeafa41a"},"embed":"\u003ciframe width=\"100%\" height=\"300\" scrolling=\"no\" frameborder=\"no\" src=\"https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/138383853%3Fsecret_token%3Ds-rFyvm&amp;auto_play=false&amp;hide_related=false&amp;visual=true\"\u003e\u003c/iframe\u003e"},{"id":137943951,"title":"Ambience","soundCloudMeta":{"state":"finished","streamUrl":"https://api.soundcloud.com/tracks/137943951/stream","downloadUrl":"https://api.soundcloud.com/tracks/137943951/download","playbackCount":97,"duration":600167,"createdAt":"2014/03/04 22:31:53 +0000","downloadable":"True"},"customTrackMeta":{"trackId":137943951,"username":"daniel.werthen@netlight.com","songTitle":null,"stageName":"OneHour","musicians":"David Werthén","composers":"David Werthén","docType":"track","_id":"55fb4fe0e04322819778ddb15ddb3923","_rev":"1-0ede90b321a61d3a861ddef448d19e6f"},"embed":"\u003ciframe width=\"100%\" height=\"300\" scrolling=\"no\" frameborder=\"no\" src=\"https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/137943951%3Fsecret_token%3Ds-WLLm0&amp;auto_play=false&amp;hide_related=false&amp;visual=true\"\u003e\u003c/iframe\u003e"}]);
	
		$httpBackend.whenGET('/api/vote/user').respond([{
			"username":"patric.ogren@netlight.com",
			"points":1,
			"trackId":139523902
			},
			{
			"username":"patric.ogren@netlight.com",
			"points":2,
			"trackId":138384027
			},
			{
			"username":"patric.ogren@netlight.com",
			"points":3,
			"trackId":137943951
			}]);

		$httpBackend.whenPOST('/api/vote').respond({});
	});
