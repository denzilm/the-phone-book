import React from 'react';
import { Link } from 'react-router-dom';

import './Header.css';

export default () => {
	return (
		<header className='header'>
			<div className='header__headline'>
				<span className='text-uppercase  font-weight-light'>
					Tired of managing too many contacts
				</span>
				<span className='font-weight-light'>
					Consolidate with{' '}
					<span className='brand'>ThePhoneBook</span>
				</span>
				<div className='row'>
					<Link to='/login' className='btn btn-lg btn-primary mr-2'>
						Sign In
					</Link>
					<Link to='/signup' className='btn btn-lg btn-secondary'>
						Sign Up
					</Link>
				</div>
			</div>
		</header>
	);
};
