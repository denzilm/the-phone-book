import React from 'react';
import ReactDOM from 'react-dom';

const Modal = props => {
	return ReactDOM.createPortal(
		<div id='modal' className='modal fade' tabIndex='-1' role='dialog' onClick={evt => evt.stopPropagation()}>
			<div className='modal-dialog modal-dialog-centered' role='document'>
				<div className='modal-content'>
					<div className='modal-header'>
						<h5 className='model-title'>{props.title}</h5>
						<button type='button' className='close' data-dismiss='modal' aria-label='Close'>
							<span aria-hidden='true'>&times;</span>
						</button>
					</div>
					<div className='model-body p-3 lead'>{props.content}</div>
					<div className='modal-footer'>{props.actions}</div>
				</div>
			</div>
		</div>,
		document.getElementById('modal-root')
	);
};

export default Modal;
