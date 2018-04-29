import * as React from "react"
import styled from "styled-components";
import styles from "constants/styles" ;

export interface ErrorSummaryProps {
    errors: string[],
    className?: string
}

function renderError(error: string, index: number) : JSX.Element {
    return (
        <li key={index}>
            {error}
        </li>
    )
}

const ErrorSummary: React.SFC<ErrorSummaryProps> = (props) => {
    return (
        <StyledDiv className={props.className}>
            <ul>
                {props.errors.map(renderError)} 
            </ul>
        </StyledDiv>
    )
}

const StyledDiv = styled.div`
    border-radius: 5px;
    border: 1px solid ${styles.colors.red};
    color: ${styles.colors.red};
    background-color: #f8d7da;
    font-size: 14px;
    padding: 12px 20px;

    ul {
        margin: 0;
        padding: 0;

        li {
            margin-left: 10px;
        }
    }
`
export default ErrorSummary;