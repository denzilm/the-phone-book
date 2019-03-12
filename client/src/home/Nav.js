import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import classNames from 'classnames';
import { connect } from 'react-redux';

const Nav = props => {
	return (
		<nav className='navbar navbar-expand-sm navbar-dark bg-primary'>
			<div className='container'>
				<Link className='navbar-brand brand text-md' to='/'>
					ThePhoneBook
				</Link>
				{props.auth.authenticated && (
					<div>
						<Link className='nav-link text-normal' to='/manage/phonebooks'>
							Phone Books
						</Link>
					</div>
				)}
				<button className='navbar-toggler' type='button' data-toggle='collapse' data-target='#mobile-nav'>
					<span className='navbar-toggler-icon' />
				</button>
				{!props.auth.authenticated && (
					<>
						<div className='collapse navbar-collapse' id='mobile-nav'>
							<ul className='navbar-nav ml-auto'>
								<li
									className={classNames('nav-item', {
										active: props.location.pathname === '/signup'
									})}
								>
									<Link className='nav-link text-normal' to='/signup'>
										Sign Up
									</Link>
								</li>
								<li
									className={classNames('nav-item', { active: props.location.pathname === '/login' })}
								>
									<Link className='nav-link text-normal' to='/login'>
										Login
									</Link>
								</li>
							</ul>
						</div>
					</>
				)}
			</div>
		</nav>
	);
};

export default connect(state => ({ auth: state.auth }))(withRouter(Nav));
