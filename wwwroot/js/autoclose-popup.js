
var autoCloseTimeout;

// modal example
$('#venkatapur_unitno_2').on('click', function(e) {
  e.preventDefault();
  autoClose({
    target: $('#BlockModal2'),
  });
});

function autoClose(options) { // eslint-disable-line
    // set defaults
    const defaults = {
      contextual: 'success',
      timeout: 3000
    };
    // apply options
    const $obj = options.target;
    const contextual = options.contextual || defaults.contextual;
    const timeout = options.timeout || defaults.timeout;
    let type = 'modal';
  
    // trigger modal or show alert
    type === 'modal' ? $obj.modal('show') : $obj.addClass('is-shown');
  
    clearTimeout(autoCloseTimeout); // eslint-disable-line
  
    autoCloseTimeout = setTimeout(function() { // eslint-disable-line
      type === 'modal' ? $obj.modal('hide') : $obj.removeClass('is-shown');
    }, timeout);
  }