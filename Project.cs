﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ManagerProject
{
    public class Project : ManagerBase
    {
        public enum ProjestStatus {start, active, suspended, done }

        public const string XmlAttributeEmployeeID = "employeeID";
        public const string XMLNodeProject = "Project";
        public const string XMLNodeAttributeID = "projectID";
        public const string XMLProjectAttributeName = "name";
        public const string XMLProjectAttributeCost = "cost";
        public const string XMLProjectAttributeStatus = "status";
        public const string XMLProjectAttributeCustomer = "customer";
        public const string XMLProjectAttributeDateAgreement = "dateAgreement";
        public const string DateFormat = "dd.MM.yyyy HH:mm";
        public const string XMLNodeEmployee = "Employee";
    
        public int ProjectID { get; private set; }
        public string Name { get; set; }
        public DateTime DateAgreement { get; private set; }
        public float Cost { get; set; }
        public ProjestStatus Status { get; set; }
        public int CustomerID { get; set; }
        public List<int> EmployeesID{ get; set; }

        public Project(int projectID, string name, DateTime dateAgreement, 
                       float cost, ProjestStatus status , 
                       int customerID,List<int> employeesID, Manager manager) : base(manager)
        {
            this.ProjectID = projectID;
            this.Name = name;
            this.DateAgreement = dateAgreement;
            this.Cost = cost;
            this.Status = status;
            this.CustomerID = customerID;
            this.EmployeesID = employeesID;
        }

        public Project(Manager manager) : base(manager)
        {
            this.ProjectID = 1;
            this.Name = "";
            this.DateAgreement = new DateTime { };
            this.Cost = 0;
            this.Status = new ProjestStatus { };
            this.CustomerID = 1;
            this.EmployeesID = new List<int> { };
        }

        public void LoadFromNode(XmlNode projectNode)
        {
            this.ProjectID = XmlNodeHelper.GetNodeAttributeI(projectNode, XMLNodeAttributeID);
            this.Name = XmlNodeHelper.GetNodeAttribute(projectNode, XMLProjectAttributeName);
            this.DateAgreement = XmlNodeHelper.GetNodeAttributeDT(projectNode, XMLProjectAttributeDateAgreement,DateFormat);
            this.Cost = XmlNodeHelper.GetNodeAttributeF(projectNode, XMLProjectAttributeCost);
            this.Status = (ProjestStatus)Enum.Parse(typeof(ProjestStatus), XmlNodeHelper.GetNodeAttribute(projectNode, XMLProjectAttributeStatus).ToString());
            this.CustomerID = XmlNodeHelper.GetNodeAttributeI(projectNode, XMLProjectAttributeCustomer);
            for (XmlNode node = projectNode.FirstChild; node != null; node = node.NextSibling)
                this.EmployeesID.Add(XmlNodeHelper.GetNodeAttributeI(node, XmlAttributeEmployeeID));
        }

        public void SaveToNode(XmlNode projectNode)
        {
            XmlNodeHelper.SetNodeAttributeI(projectNode, XMLNodeAttributeID, this.ProjectID);
            XmlNodeHelper.SetNodeAttribute(projectNode, XMLProjectAttributeName, this.Name);
            XmlNodeHelper.SetNodeAttribute(projectNode, XMLProjectAttributeDateAgreement, this.DateAgreement.ToString(DateFormat));
            XmlNodeHelper.SetNodeAttributeF(projectNode, XMLProjectAttributeCost, this.Cost);
            XmlNodeHelper.SetNodeAttribute(projectNode, XMLProjectAttributeStatus, this.Status.ToString());
            XmlNodeHelper.SetNodeAttribute(projectNode, XMLProjectAttributeCustomer, this.CustomerID.ToString());
            
            for (int i = 0; i < this.EmployeesID.Count; i++)
            {
                XmlNode projectEmployeeNode = XmlNodeHelper.RequiredNode(projectNode, Project.XMLNodeEmployee,
                    Project.XmlAttributeEmployeeID, this.EmployeesID[i].ToString());
            }

            XmlNodeList xmlProjectList = projectNode.SelectNodes(Project.XMLNodeEmployee);
            foreach (XmlNode node in xmlProjectList)
            {
                int employeeInProjectId = XmlNodeHelper.GetNodeAttributeI(node, Project.XmlAttributeEmployeeID);
                if (this.EmployeesID.IndexOf(employeeInProjectId) < 0)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }
        }
    }
}
