import * as React from 'react';
import TextInput from 'components/shared/input/TextInput';
import Button from 'components/shared/input/Button';
import ErrorSummary from "components/shared/ErrorSummary"
import AccountApi from 'api/AccountApi'
import ValidationErrors from 'models/Shared/ValidationErrors'
import styled from "styled-components";
import styles from "constants/styles" ;

interface FieldState {
    value: string,
    error: string
}

interface LoginPageFields {
    username: FieldState,
    password: FieldState,
}

interface LoginPageState {
    fields: LoginPageFields,
    submitting: boolean,
    validationErrors: string[]
}

class LoginPage extends React.Component<any, LoginPageState> {

    constructor(props: any)
    {
        super(props);

        this.state = {
            fields: {
                username: {
                    value: "",
                    error: null
                },
                password: {
                    value: "",
                    error: null
                }
            },
            submitting: false,
            validationErrors: []
        };

        this.onTextInputChanged = this.onTextInputChanged.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    onTextInputChanged(target: string, event: any): void {
        this.setState(
            {
                ...this.state,
                fields: {
                    ...this.state.fields,
                    [target]: {
                        ...this.state.fields[target],
                        value: event.target.value
                    }
                }
            }
        )       
    }

    onSubmit() : void {

        this.setSubmitting(true);

        AccountApi.login(this.state.fields.username.value, this.state.fields.password.value)
            .then(d =>
            {
                alert("Successful login!");
                return Promise.resolve();
            })
            .catch(e => {
                if (!!e && e.errors !== undefined) {
                    this.setValidationErrors(e);
                }
                else {
                    this.setValidationErrors({
                        errors: [
                            {
                                field: null,
                                errorMessage: "Something went wrong"
                            }
                        ]
                    });
                }
            })
            .then(d => this.setSubmitting(false));      

    }

    setValidationErrors(errors: ValidationErrors)
    {
        var newFieldsState = {};
        var self = this;

        Object.keys(this.state.fields).forEach(function (key, index) {
            var error = errors.errors.find(e => e.field === key);

            var field: FieldState = {
                value: self.state.fields[key].value,
                error: !!error ? error.errorMessage : null
            };

            newFieldsState[key] = field;
        });

        var validationErrors = errors.errors.filter(e => !e.field).map(e => e.errorMessage);

        this.setState({
                ...this.state,
                fields: newFieldsState as LoginPageFields,
                validationErrors: validationErrors
            });
    }

    setSubmitting(submitting: boolean): void {
        this.setState(
            {
                ...this.state,
                submitting: submitting
            }
        );
    }

    public render() {
        return <PageDiv>

            <ContainerDiv>

                <TextInput value={this.state.fields.username.value} error={this.state.fields.username.error} icon="fa-user" onChange={this.onTextInputChanged} placeholder="Username" name="username" disabled={this.state.submitting} />

                <TextInput value={this.state.fields.password.value} error={this.state.fields.password.error} icon="fa-lock" password={true} onChange={this.onTextInputChanged} placeholder="Password" name="password" disabled={this.state.submitting} />

                {this.state.validationErrors.length != 0 &&
                    <StyledErrorSummary errors={this.state.validationErrors}/>
                }

                <FullWidthButton text="Log in" color="green" loading={this.state.submitting} onClicked={this.onSubmit} />

            </ContainerDiv>

        </PageDiv>;
    }
}

const StyledErrorSummary = styled(ErrorSummary)`
    margin-bottom: 20px;
`

const FullWidthButton = styled(Button)`
    width: 100%;
`

const PageDiv = styled.div`
    padding: 0 20px;
    overflow: hidden;
    margin: 0 auto;
    display: table;
    height: 100%;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        padding: 0 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        padding: 0 10px;
    }
`

const ContainerDiv = styled.div`
    display: table-cell;
    vertical-align: middle;
    width: 350px;
    margin: 0 auto;
    color: #FFF;
`


export default LoginPage;