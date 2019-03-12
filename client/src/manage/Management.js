import React from 'react';
import { Link, Route, Switch } from 'react-router-dom';
import classnames from 'classnames';
import { connect } from 'react-redux';

import { logoutUser } from '../auth/actions';
import PhoneBooks from './phonebooks';
import PhoneBookDetail from './phonebook/PhoneBookDetail';
import PhoneBookEntry from './phonebook/entries/PhoneBookEntry';
import PhoneBookCreate from './phonebook/PhoneBookCreate';

const Management = props => {
	return (
		<>
			<div className='navbar navbar-expand-sm navbar-dark bg-primary'>
				<div className='container'>
					<Link className='navbar-brand brand text-md' to='/'>
						ThePhoneBook
					</Link>
					<button className='navbar-toggler' type='button' data-toggle='collapse' data-target='#mobile-nav'>
						<span className='navbar-toggler-icon' />
					</button>
					<div className='collapse navbar-collapse' id='mobile-nav'>
						<div className='navbar-nav ml-auto'>
							<button className={classnames('btn btn-warning btn-lg')} onClick={() => props.logoutUser()}>
								{'Logout'}
							</button>
						</div>
					</div>
				</div>
			</div>
			<Route path='/manage/phonebooks' component={PhoneBooks} />
			<Switch>
				<Route exact path='/manage/phonebook/new' component={PhoneBookCreate} />
				<Route exact path='/manage/phonebook/:id' component={PhoneBookDetail} />
			</Switch>
			<Route exact path='/manage/phonebook/:phoneBookId/entries/:id' component={PhoneBookEntry} />
		</>
	);
};

export default connect(
	null,
	{ logoutUser }
)(Management);
