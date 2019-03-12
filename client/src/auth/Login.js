import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Form, Field } from 'react-final-form';
import { connect } from 'react-redux';

import Input from '../common//components/form/Input';
import Nav from '../home/Nav';
import * as Validators from '../common/validation';
import { loginUser } from './actions';

class Login extends Component {
	static propTypes = {
		auth: PropTypes.object.isRequired,
		loginUser: PropTypes.func.isRequired
	};

	onSubmit = ({ email, password }) => {
		this.props.loginUser(email, password);
	};

	componentDidUpdate() {
		if (this.props.auth.authenticated) {
			this.props.history.push('/manage/phonebooks');
		}
	}

	render() {
		const errors =
			this.props.errors.length > 0 ? this.props.errors.map(e => <li key={e.code}>{e.description}</li>) : null;
		return (
			<div style={{ height: '92.5vh' }}>
				<Nav />
				<div className='container mt-5'>
					<div className='row'>
						<div className='col-md-8 offset-md-2'>
							<h1 className='display-4 text-center'>Sign In</h1>
							<p className='lead text-center'>
								Sign in to your <span className='brand'>ThePhoneBook</span> account
							</p>
							{errors && <div className='text-danger font-weight-bold text-center p-3'>{errors}</div>}
							<Form
								onSubmit={this.onSubmit}
								render={({ handleSubmit, submitting, pristine, invalid }) => (
									<form onSubmit={handleSubmit}>
										<Field
											name='email'
											type='email'
											placeholder='Email Address'
											component={Input}
											validate={Validators.composeValidators(
												Validators.required('Email Address'),
												Validators.isEmail
											)}
										/>
										<Field
											name='password'
											type='password'
											placeholder='Password'
											component={Input}
											validate={Validators.required('Password')}
										/>
										<input
											type='submit'
											className='btn btn-primary btn-lg btn-block mt-4'
											disabled={submitting || pristine || invalid}
											value='Sign In'
										/>
									</form>
								)}
							/>
						</div>
					</div>
				</div>
			</div>
		);
	}
}

const mapStateToProps = state => ({ errors: state.errors, auth: state.auth });

export default connect(
	mapStateToProps,
	{ loginUser }
)(Login);
