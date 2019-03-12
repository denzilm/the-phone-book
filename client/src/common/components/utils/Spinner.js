import React from 'react';
import ReactDOM from 'react-dom';

import './Spinner.css';
import loader from '../../../img/tail-spin.svg';

const renderContent = () => {
	return (
		<div className='spinner'>
			<img src={loader} style={{ width: '10%'}} alt='Spinner' />
			<p className='lead mt-5'>Loading your content...</p>
		</div>
	);
};

export default () => {
	return ReactDOM.createPortal(renderContent(), document.getElementById('spinner-root'));
};
