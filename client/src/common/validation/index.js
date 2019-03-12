import Validator from 'validator';
import _ from 'lodash';

const required = field => value => (value ? undefined : `${field} is required`);
const isEmail = value => (!_.isEmpty(value) && Validator.isEmail(value) ? undefined : 'Email is in an invalid format');
const isPhoneNumber = value =>
	// eslint-disable-next-line no-useless-escape
	/^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$/g.test(value) ? undefined : 'Phone number is in an invalid format';
const minLength = min => value => (value.length >= min ? undefined : `Should be greater than ${min} characters`);
const maxLength = max => value => (value.length < max ? undefined : `Should be less than ${max} characters`);
const containsDigits = value => (/\d+/.test(value) ? undefined : `Must have at least one digit ('0' - '9')`);
const containsNonAlphaNumeric = value =>
	/[^0-9a-zA-Z]+/.test(value) ? undefined : `Must have at least one non alphanumeric character`;
const containsUpperCase = value => (/[A-Z]+/.test(value) ? undefined : `Must have at least one uppercase ('A' - 'Z')`);
const containsLowerCase = value => (/[a-z]+/.test(value) ? undefined : `Must have at least one lowercase ('a' - 'z')`);

const composeValidators = (...validators) => value =>
	validators.reduce((error, validator) => error || validator(value), undefined);

export {
	required,
	minLength,
	maxLength,
	composeValidators,
	isEmail,
	isPhoneNumber,
	containsDigits,
	containsNonAlphaNumeric,
	containsUpperCase,
	containsLowerCase
};
