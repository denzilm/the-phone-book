import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { Form, Field } from 'react-final-form';
import Validator from 'validator';

import Input from '../common/components/form//Input';
import * as Validators from '../common/validation';
import { registerUser } from './actions';

import Nav from '../home/Nav';

class Register extends Component {
	static propTypes = {
		errors: PropTypes.array,
		registerUser: PropTypes.func.isRequired
	};

	onSubmit = ({ firstName, lastName, email, password, confirmPassword }) => {
		const newUser = {
			firstName,
			lastName,
			email,
			password,
			confirmPassword
		};
		this.props.registerUser(newUser);
	};

	render() {
		const errors =
			this.props.errors.length > 0 ? this.props.errors.map(e => <li key={e.code}>{e.description}</li>) : null;

		return (
			<div style={{ height: '92.5vh' }}>
				<Nav />
				<div className='container mt-5'>
					<div className='row'>
						<div className='col-md-8 offset-md-2'>
							<h1 className='display-4 text-center'>Sign Up</h1>
							<p className='lead text-center'>
								Create your <span className='brand'>ThePhoneBook</span> account
							</p>
							{errors && <div className='text-danger font-weight-bold text-center p-3'>{errors}</div>}
							<Form
								onSubmit={this.onSubmit}
								validate={values => {
									const errors = {};

									if (values.confirmPassword && values.password) {
										if (!Validator.equals(values.confirmPassword, values.password)) {
											errors.confirmPassword = 'Passwords does not match';
										}
									}

									return errors;
								}}
								render={({ handleSubmit, submitting, pristine, invalid }) => (
									<form onSubmit={handleSubmit}>
										<Field
											name='firstName'
											placeholder='First Name'
											component={Input}
											validate={Validators.required('First Name')}
										/>
										<Field
											name='lastName'
											placeholder='Last Name'
											component={Input}
											validate={Validators.required('Last Name')}
										/>
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
											validate={Validators.composeValidators(
												Validators.required('Password'),
												Validators.minLength(6),
												Validators.containsDigits,
												Validators.containsNonAlphaNumeric,
												Validators.containsLowerCase,
												Validators.containsUpperCase
											)}
										/>
										<Field
											name='confirmPassword'
											type='password'
											placeholder='Confirm Password'
											component={Input}
											validate={Validators.required('Confirm Password')}
										/>
										<input
											type='submit'
											className='btn btn-primary btn-lg btn-block mt-4'
											disabled={submitting || pristine || invalid}
											value='Sign Up'
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

export default connect(
	({ errors }) => ({ errors }),
	{ registerUser }
)(Register);
