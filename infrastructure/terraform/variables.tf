variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
  default     = "app-rg"
}

variable "location" {
  description = "Azure region to deploy resources"
  type        = string
  default     = "northeurope"
  validation {
    condition     = can(regex("^[a-z]+[a-z0-9]+$", var.location))
    error_message = "Location must be a valid Azure region name."
  }
}

variable "vm_size" {
  description = "Size of the VM"
  type        = string
  default     = "Standard_B1s"
  validation {
    condition     = can(regex("^Standard_", var.vm_size))
    error_message = "VM size must be a valid Azure VM size starting with 'Standard_'."
  }
}

variable "admin_username" {
  description = "Admin username for the VM"
  type        = string
  default     = "azureuser"
  validation {
    condition     = length(var.admin_username) >= 3
    error_message = "Admin username must be at least 3 characters long."
  }
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