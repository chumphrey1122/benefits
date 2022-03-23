import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Table, Col, Row, Input } from 'reactstrap'

export class EmployeeAndDependents extends Component {
    static displayName = EmployeeAndDependents.name;

    // TODO: Move the Payroll Preview to a separate Component
    constructor(props) {
        super(props);
        // How to format a Javascript number as currency: https://stackoverflow.com/questions/149055/how-to-format-numbers-as-currency-strings
        this.moneyFormatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });
        this.state = {
            employee: { dependents: [] },
            payroll: {},
            firstName: "",
            lastName:""};
    }

    componentDidMount() {
        this.populateEmployees();
        this.getPayroll();
    }

    render() {
        return (
            <div>
                <Row>
                    <Col><h2>Employee Details</h2></Col>
                </Row>
                <Row>
                    <Col>
                        <dl>
                            <dt>Name:</dt><dd>{this.state.employee.lastName}, {this.state.employee.firstName}</dd>
                            <dt>Pay Per Paycheck:</dt><dd>{this.moneyFormatter.format(this.state.employee.payRate)} </dd>
                        </dl>
                    </Col>
                </Row>
                <Row>
                    <Col><h3>Dependents</h3></Col>
                </Row>
                <Row>
                    <Col>
                        <Table striped={true}>
                            <thead>
                                <tr>
                                    <th>First Name</th><th>Last Name</th><th></th>
                                </tr>
                            </thead>
                            <tbody>
                                {this.state.employee.dependents.map((dependent, i) =>
                                    <tr key={i}>
                                        <td>{dependent.firstName}</td>
                                        <td>{dependent.lastName}</td>
                                        <td><div className="clickable" onClick={() => this.deleteDependent(dependent.id)}>delete</div></td>
                                    </tr>)}
                                
                                <tr>
                                    <td><Input value={this.state.firstName} onChange={e => this.setState({ firstName: e.target.value })} type="text" placeholder="First Name"/></td>
                                    <td><Input value={this.state.lastName} onChange={e => this.setState({ lastName: e.target.value })} type="text" placeholder="Last Name"/></td>
                                    <td><Button disabled={!this.state.firstName || !this.state.lastName} onClick={() => this.addDependent()} color="secondary">Add New</Button></td>
                                </tr>
                            </tbody>
                        </Table>
                    </Col>
                </Row>

                <Row>
                    <Col><h4>Payroll Preview</h4></Col>
                </Row>
                <Row>
                    <Col>
                        <Table bordered={true}>
                            
                            <tbody>
                                <tr><th>Pay/ Period</th><td>{this.moneyFormatter.format(this.state.payroll.payPerPeriod)}</td></tr>
                                <tr><th>Member benefits cost</th><td>{this.moneyFormatter.format(-this.state.payroll.baseBenefitsCost)}</td></tr>
                                <tr><th>Dependent benefits cost</th><td>{this.moneyFormatter.format(-this.state.payroll.dependentCost)}</td></tr>
                                <tr><th>Total discounts</th><td>{this.moneyFormatter.format(this.state.payroll.baseDiscount + this.state.payroll.dependentDiscount)}</td></tr>
                                <tr><th>Net pay</th><td>{this.moneyFormatter.format(this.state.payroll.netPay)}</td></tr>
                            </tbody>
                        </Table>
                    </Col>
                </Row>
            </div>

        );
    }

    async addDependent() {
        // TODO: rather than copying the standard "headers" lots of places in the code, we may want to move the fetch call to 
        // a reusable JavaScript class (service) that can will correctly provide the headers, etc. This will be very important
        // if we later support authentication, since we'd need to add a JWT token to the headers
        const response = await fetch('api/employees/' + encodeURIComponent(this.props.match.params.id) + '/dependents', {
            method: 'POST',
            headers: {'Content-Type':'application/json'},
            body: JSON.stringify({
                firstName: this.state.firstName,
                lastName: this.state.lastName
            })
        });
        if (response.status !== 200) {
            alert("There was a problem adding the dependent");
        }
        else {
            const data = await response.json();
            var employee = this.state.employee;
            employee.dependents.push(data);
            this.setState({ employee: employee, firstName: "", lastName: "" });
            await this.getPayroll();
        }
    }

    async deleteDependent(id) {
        if (!window.confirm("Do you want to delete this dependent?"))
            return;
        const response = await fetch('api/employees/' + encodeURIComponent(this.props.match.params.id) + '/dependents/' + encodeURIComponent(id), {
            method: 'DELETE'
        });
        if (response.status !== 200) {
            alert("There was a problem deleting the dependent");
        }
        else {
            // Remove the specific dependent
            var employee = this.state.employee;
            employee.dependents = employee.dependents.filter(dep => dep.id != id);
            this.setState({ employee: employee });
            await this.getPayroll();
        }
        return;
    }

    async populateEmployees() {
        const response = await fetch('api/employees/' + encodeURIComponent(this.props.match.params.id));
        if (response.status !== 200) {
            alert("There was a problem getting the employee");
        }
        else {
            const data = await response.json();
            this.setState({ employee: data });
        }
    }

    async getPayroll() {
        const response = await fetch('api/payroll/employees/' + encodeURIComponent(this.props.match.params.id));
        if (response.status !== 200) {
            alert("There was a problem getting the employee payroll");
        }
        else {
            const data = await response.json();
            this.setState({ payroll: data });
        }
    }
}
