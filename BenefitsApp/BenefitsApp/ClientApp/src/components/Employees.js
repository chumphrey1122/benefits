import React, { Component } from 'react';
import { Table, Col, Row, Input, Button } from 'reactstrap'

export class Employees extends Component {
    static displayName = Employees.name;

    constructor(props) {
        super(props);
        this.state = { employees: [], firstName: "", lastName: "" };
    }

    componentDidMount() {
        this.populateEmployees();
    }

    render() {
        return (
            <div>
                <Row>
                    <Col><h2>Employees</h2></Col>
                </Row>
                <Row>
                    <Col>
                        <Table striped={true} hover={true}>
                            <thead>
                                <tr>
                                    <th>First Name</th><th>Last Name</th><th></th>
                                </tr>
                            </thead>
                            <tbody> 
                                {this.state.employees.map((employee, i) => <tr className="clickableRow" key={i} onClick={()=> this.goToEmployee(employee.id)}><td>{employee.firstName}</td><td> {employee.lastName}</td></tr>)}
                                <tr>
                                    <td><Input value={this.state.firstName} onChange={e => this.setState({ firstName: e.target.value })} type="text" placeholder="First Name" /></td>
                                    <td><Input value={this.state.lastName} onChange={e => this.setState({ lastName: e.target.value })} type="text" placeholder="Last Name" /></td>
                                    <td><Button disabled={!this.state.firstName || !this.state.lastName} onClick={() => this.addEmployee()} color="secondary">Add New</Button></td>
                                </tr>
                            </tbody>
                        </Table>
                    </Col>
                </Row>
            </div>
        );
    }

    goToEmployee(id) {
        this.props.history.push("/employee/" + encodeURIComponent(id));
    }

    async populateEmployees() {
        const response = await fetch('api/employees');
        if (response.status !== 200) {
            alert("There was a problem getting the employees");
        }
        else {
            const data = await response.json();
            this.setState({ employees: data });
        }
    }

    async addEmployee() {
        const response = await fetch('api/employees/', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                firstName: this.state.firstName,
                lastName: this.state.lastName
            })
        });
        if (response.status !== 200) {
            alert("There was a problem adding the employee");
        }
        else {
            const data = await response.json();
            var employees = this.state.employees;
            employees.push(data);
            this.setState({ employees: employees, firstName: "", lastName: "" });
        }
    }
}
