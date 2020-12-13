			// Bind normal buttons
			Ladda.bind( 'div:not(.progress-demo) .ladda-button02', { timeout: 2000 } );

			// Bind progress buttons and simulate loading progress
			Ladda.bind( '.progress-demo .ladda-button02', {
				callback: function( instance ) {
					var progress = 0;
					var interval = setInterval( function() {
						progress = Math.min( progress + Math.random() * 0.1, 1 );
						instance.setProgress( progress );

						if( progress === 1 ) {
							instance.stop();
							clearInterval( interval );
						}
					}, 2000 );
				}
			} );