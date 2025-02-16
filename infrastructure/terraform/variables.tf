variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "app-rg"
}

variable "location" {
  description = "Azure region to deploy resources"
  type        = string
  default     = "northeurope"
}

variable "vm_size" {
  description = "Size of the VM"
  type        = string
  default     = "Standard_B1s"
}

variable "admin_username" {
  description = "Admin username for the VM"
  type        = string
  default     = "azureuser"
}

variable "environment" {
  description = "Environment (dev, staging, prod)"
  type        = string
  default     = "dev"
}

variable "app_ports" {
  description = "Ports to open in the security group"
  type        = list(number)
  default     = [22, 80, 5129, 27017]
}

variable "tags" {
  description = "Tags to apply to all resources"
  type        = map(string)
  default     = {
    Environment = "dev"
    ManagedBy   = "terraform"
  }
}

variable "deployment_timestamp" {
  description = "Timestamp for deployment"
  type        = string
}